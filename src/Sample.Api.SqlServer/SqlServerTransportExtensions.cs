namespace Sample.Api.SqlServer;

using MassTransit;
using Microsoft.Data.SqlClient;


public static class SqlServerTransportExtensions
{
    /// <summary>
    /// Works some shenanigans to get all the host options configured for the Postgresql transport
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="create"></param>
    /// <param name="delete"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureSqlServerTransport(this IServiceCollection services, string? connectionString, bool create = true,
        bool delete = false)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

        services.AddOptions<SqlTransportOptions>().Configure(options =>
        {
            options.Host = "localhost";
            options.Database = builder.InitialCatalog ?? "sample";
            options.Schema = "transport";
            options.Role = "transport";
            options.Username = "masstransit";
            options.Password = "H4rd2Gu3ss!";
            options.AdminUsername = builder.UserID ?? "sa";
            options.AdminPassword = builder.Password ?? "Password12!";
        });

        services.AddSqlServerMigrationHostedService(create, delete);

        return services;
    }
}