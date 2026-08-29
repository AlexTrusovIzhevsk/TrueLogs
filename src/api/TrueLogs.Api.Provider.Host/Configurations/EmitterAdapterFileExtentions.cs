using TrueLogs.Contract.Clients;
using TrueLogs.Emitter.Adapter.File.LogFileConfig;
using TrueLogs.Emitter.Adapter.File.LogFileManagers;
using TrueLogs.Emitter.Adapter.File.LogFileParser;
using TrueLogs.Emitter.Adapter.File.LogFileReader;
using TrueLogs.Emitter.Adapter.File.LogFileWatcherds;
using TrueLogs.Emitter.Adapter.File.PositionTrackers;
using TrueLogs.Emitter.Client;

namespace TrueLogs.Api.Provider.Host.Configurations;

public static class EmitterAdapterFileExtentions
{

    public static WebApplicationBuilder AddEmitterAdapterFile(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton<ILogFileConfigRedaer>(_ => new LogFileConfigRedaer(new LogFileConfig
            {
                Path = LoggingExtentions.ProviderRowPath,
                Key = LoggingExtentions.ProviderStructuredPath,
            }))
            .AddSingleton<IPositionTracker, InMemoryPositionTracker>()
            .AddSingleton<ILogFileWatcher, LogFileWatcher>()
            .AddSingleton<ILogFileReader, LogFileReader>()
            .AddSingleton<ILogFileParser, RowLogFileParser>()
            .AddSingleton<ILogEmitterClient, LogEmitterClient>()
            .AddSingleton<ILogFileManager, LogFileManager>()
            .AddSingleton(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5265/") }); ;

        return builder;
    }
}
