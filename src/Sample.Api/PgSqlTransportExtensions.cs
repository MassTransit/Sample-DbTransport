namespace Sample.Api;

using MassTransit;
using Npgsql;


public static class PgSqlTransportExtensions
{
    /// <summary>
    /// Works some shenanigans to get all the host options configured for the Postgresql transport
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="create"></param>
    /// <param name="delete"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigurePostgresTransport(this IServiceCollection services, string? connectionString, bool create = true,
        bool delete = false)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        services.AddOptions<SqlTransportOptions>().Configure(options =>
        {
            options.Host = builder.Host ?? "localhost";
            options.Database = builder.Database ?? "sample";
            options.Schema = "transport";
            options.Role = "transport";
            options.Username = "masstransit";
            options.Password = "H4rd2Gu3ss!";
            options.AdminUsername = builder.Username;
            options.AdminPassword = builder.Password;
        });

        services.AddPostgresMigrationHostedService(create, delete);

        return services;
    }
}