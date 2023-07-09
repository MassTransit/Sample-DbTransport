using System.Reflection;
using MassTransit;
using MassTransit.DependencyInjection;
using MassTransit.Internals;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NSwag;
using Sample.Components;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("Db");

builder.Services.AddDbContext<SampleDbContext>(x =>
{
    x.UseNpgsql(connectionString, options =>
    {
        options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        options.MigrationsHistoryTable("ef_migration_history");
    });
});
builder.Services.AddHostedService<MigrationHostedService<SampleDbContext>>();


builder.Services.AddSingleton<IEndpointAddressProvider, DbEndpointAddressProvider>();

builder.Services.ConfigurePostgresTransport(connectionString);
builder.Services.AddMassTransit(x =>
{
    x.SetEntityFrameworkSagaRepositoryProvider(r =>
    {
        r.ExistingDbContext<SampleDbContext>();
        r.UsePostgres();
    });

    x.AddSagaRepository<JobSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });
    x.AddSagaRepository<JobTypeSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });
    x.AddSagaRepository<JobAttemptSaga>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<SampleDbContext>();
            r.UsePostgres();
        });

    x.SetKebabCaseEndpointNameFormatter();

    x.AddEntityFrameworkOutbox<SampleDbContext>(o =>
    {
        o.UsePostgres();
    });

    x.AddConfigureEndpointsCallback((context, _, cfg) =>
    {
        cfg.UseDelayedRedelivery(r =>
        {
            r.Handle<LongTransientException>();
            r.Interval(10000, 15000);
        });

        cfg.UseMessageRetry(r =>
        {
            r.Handle<TransientException>();
            r.Interval(25, 50);
        });

        cfg.UseEntityFrameworkOutbox<SampleDbContext>(context);
    });

    x.AddConsumersFromNamespaceContaining<ComponentsNamespace>();
    x.AddActivitiesFromNamespaceContaining<ComponentsNamespace>();
    x.AddSagaStateMachinesFromNamespaceContaining<ComponentsNamespace>();

    x.UsingPostgres((context, cfg) =>
    {
        cfg.UseDbMessageScheduler();

        cfg.AutoStart = true;

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {
        options.WaitUntilStarted = true;
        options.StartTimeout = TimeSpan.FromSeconds(10);
        options.StopTimeout = TimeSpan.FromSeconds(30);
        options.ConsumerStopTimeout = TimeSpan.FromSeconds(10);
    });
builder.Services.AddOptions<HostOptions>()
    .Configure(options => options.ShutdownTimeout = TimeSpan.FromMinutes(1));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(cfg => cfg.PostProcess = d =>
{
    d.Info.Title = "MassTransit Database Transport Sample";
    d.Info.Contact = new OpenApiContact
    {
        Name = "Chris Patterson",
        Email = "support@masstransit.io"
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseRouting();
app.MapControllers();

static Task HealthCheckResponseWriter(HttpContext context, HealthReport result)
{
    context.Response.ContentType = "application/json";

    return context.Response.WriteAsync(result.ToJsonString());
}

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseWriter
});

app.MapHealthChecks("/health/live", new HealthCheckOptions { ResponseWriter = HealthCheckResponseWriter });

app.Run();