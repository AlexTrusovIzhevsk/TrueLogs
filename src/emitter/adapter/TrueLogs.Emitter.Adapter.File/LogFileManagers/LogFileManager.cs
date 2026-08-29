
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using TrueLogs.Contract.Clients;
using TrueLogs.Emitter.Adapter.File.LogFileParser;
using TrueLogs.Emitter.Adapter.File.LogFileReader;
using TrueLogs.Emitter.Adapter.File.LogFileWatcherds;

namespace TrueLogs.Emitter.Adapter.File.LogFileManagers;

public class LogFileManager : ILogFileManager
{
    //private readonly string _logFilePath;
    private readonly ILogFileReader _logFileReader;
    private readonly ILogFileWatcher _logFileWatcher;
    private readonly ILogFileParser _logFileParser;
    private readonly ILogEmitterClient _logEmitterClient;
    private readonly ILogger<LogFileManager> _logger;
    private readonly Channel<bool> _changeChannel;
    private CancellationTokenSource _cancellationTokenSource;

    public LogFileManager(
        //string path, 
        ILogFileReader logFileReader, 
        ILogFileWatcher logFileWatcher,
        ILogFileParser logFileParser,
        ILogEmitterClient logEmitterClient,
        ILogger<LogFileManager> logger)
    {
        //_logFilePath = path;
        _logFileReader = logFileReader;
        _logFileWatcher = logFileWatcher;
        _logFileParser = logFileParser;
        _logEmitterClient = logEmitterClient;
        _logger = logger;
    }

    public Task StartWatch(CancellationToken token)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        _logFileWatcher.FileLogChange += OnChanged;
        return Task.CompletedTask;
    }

    // TODO Временное решение что бы запись новых логов не инициировала чтение файла когда файл читается 
    private Task? currentTask;

    private Task OnChanged(object sender, LogFileChangeEventArgs fileSystemEventArgs)
    {
        if (currentTask is not null)
        {
            return currentTask;
        }
        else
        {
            currentTask = HandleChange();
            currentTask.ContinueWith(t => currentTask = null);
            return currentTask;
        }
    }

    private async Task HandleChange()
    {
        await foreach (var log in _logFileReader.ReadLogsAsync(_cancellationTokenSource.Token))
        {
            var logEmitterContract = await _logFileParser.Parse(log, _cancellationTokenSource.Token);
            // TODO отправлять порциями
            await _logEmitterClient.Emit([logEmitterContract], _cancellationTokenSource.Token);
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        _logFileWatcher.Dispose();
        _logFileReader.Dispose();
    }
}
