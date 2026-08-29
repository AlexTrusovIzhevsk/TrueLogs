using TrueLogs.Emitter.Adapter.File.LogFileManagers;

namespace TrueLogs.Api.Provider.Host.Backgrounds;

public class FileWatcherBackgroundService : BackgroundService
{
    private readonly ILogFileManager _logFileManager;
    private readonly ILogger<FileWatcherBackgroundService> _logger;

    public FileWatcherBackgroundService(ILogFileManager logFileManager, ILogger<FileWatcherBackgroundService> logger) 
    {
        _logger = logger;
        _logFileManager = logFileManager;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var taskCompletionSource = new TaskCompletionSource();
        stoppingToken.Register(taskCompletionSource.SetCanceled);

        _logFileManager.StartWatch(stoppingToken);

        return taskCompletionSource.Task;
    }

    public override void Dispose()
    {
        _logFileManager.Dispose();
    }
}
