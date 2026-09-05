using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrueLogs.Storage.LiteDB;
using TrueLogs.Storage.LiteDB.Logs.Repositories;
using TrueLogs.Storage.LiteDB.Logs.Managers;
using TrueLogs.Storage.Logs.Managers;
using TrueLogs.Storage.Logs.Providers;
using TrueLogs.Storage.PostgreSQL.Logs.Managers;

namespace TrueLogs.Api.Core;

public static class LogExtention
{
    public static IServiceCollection AddLogProvider(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<LogProvider>(serviceProvider =>
        {
            var (provider, connectionString) = GetProvider(serviceProvider);

            if (provider == "PostgreSQL")
            {
                var logger = serviceProvider.GetRequiredService<ILogger<PostgreSQLLogProvider>>();
                return new PostgreSQLLogProvider(connectionString, logger);
            }
            else if (provider == "LiteDB")
            {
                var repository = serviceProvider.GetRequiredService<LiteDbLogRepository>();
                var logger = serviceProvider.GetRequiredService<ILogger<LiteDBLogProvider>>();
                return new LiteDBLogProvider(repository, logger);
            }

            throw new ArgumentException($"Provider: '{nameof(provider)}'");
        });

        return services;
    }
    public static IServiceCollection AddLogManager(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ILogManager>(serviceProvider =>
        {
            var (provider, connectionString) = GetProvider(serviceProvider);

            if (provider == "PostgreSQL")
            {
                var logger = serviceProvider.GetRequiredService<ILogger<PostgreSQLLogManager>>();
                return new PostgreSQLLogManager(connectionString, logger);
            }
            else if (provider == "LiteDB")
            {
                var repository = serviceProvider.GetRequiredService<LiteDbLogRepository>();
                return new LiteDBLogManager(repository);
            }

            throw new ArgumentException($"Provider: '{nameof(provider)}'");
        });

        return services;
    }

    private static (string provider, string connectionString) GetProvider(IServiceProvider serviceProvider)
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();

        var provider = config["Storage:Provider"];
        if (string.IsNullOrEmpty(provider))
        {
            throw new ArgumentException(nameof(provider));
        }

        var connectionString = config["Storage:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException(nameof(connectionString));
        }

        return (provider, connectionString);
    }
}
