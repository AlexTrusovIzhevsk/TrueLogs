using System.IO;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using TrueLogs.Emitter.Adapter.File.LogFileConfig;

namespace TrueLogs.Emitter.Adapter.File.LogFileWatcherds;

public class LogFileWatcher : ILogFileWatcher
{
    //private readonly string _logFilePath;
    private readonly ILogFileConfigRedaer _logFileConfigRedaer;
    private readonly FileSystemWatcher _watcher;
    private readonly ILogger<LogFileWatcher> _logger;

    public LogFileWatcher(ILogFileConfigRedaer logFileConfigRedaer, ILogger<LogFileWatcher> logger)
    {
        //_logFilePath = path;
        _logFileConfigRedaer = logFileConfigRedaer;
        _logger = logger;
        var filePath = logFileConfigRedaer.GetConfig().Path;
        var dirPath = Path.GetDirectoryName(filePath);
        _watcher = new FileSystemWatcher(dirPath, "*.row.log");
        _watcher.NotifyFilter = NotifyFilters.LastWrite;
        _watcher.Changed += OnChanged;
        _watcher.Error += OnError;
        _watcher.EnableRaisingEvents = true;
    }

    public event FileLogChangeEventHandler FileLogChange;

    private void OnChanged(object sender, FileSystemEventArgs fileSystemEventArgs) 
    {
        var logFileChangeEventArgs = new LogFileChangeEventArgs() { LogFileFullPath = fileSystemEventArgs.FullPath };
        FileLogChange.Invoke(this, logFileChangeEventArgs);
    }

    private void OnError(object sender, ErrorEventArgs eventArgs)
    {
        var exception = eventArgs.GetException();
        //if (exception != null)
        //{
        //    _logger.LogError(exception, "Error on file watche");
        //}
    }

    public void Dispose()
    {
        _watcher.Dispose();
    }
}
