
using Microsoft.Extensions.Logging;
using TrueLogs.Emitter.Adapter.File.LogFileConfig;
using TrueLogs.Emitter.Adapter.File.PositionTrackers;

namespace TrueLogs.Emitter.Adapter.File.LogFileReader;

public class LogFileReader : ILogFileReader
{
    //private readonly string _logFilePath;
    private readonly IPositionTracker _positionTracker;
    private readonly ILogFileConfigRedaer _logFileConfigRedaer;
    private readonly ILogger<LogFileReader> _logger;


    public LogFileReader(
        ILogFileConfigRedaer logFileConfigRedaer, 
        IPositionTracker positionTracker, 
        ILogger<LogFileReader> logger)
    {
        //_logFilePath = path;
        _logFileConfigRedaer = logFileConfigRedaer;
        _positionTracker = positionTracker;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> ReadLogsAsync(CancellationToken token)
    {
        var logFilePath = _logFileConfigRedaer.GetConfig().Path;
        using var logFileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var currentPosition = await _positionTracker.GetCurrentPositionAsync(token);
        logFileStream.Seek(currentPosition, SeekOrigin.Begin);
        
        var reader = new StreamReader(logFileStream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line is not null)
            {
                yield return line;
            }

            currentPosition = logFileStream.Position;
            await _positionTracker.SetCurrentPositionAsync(currentPosition, token);
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
