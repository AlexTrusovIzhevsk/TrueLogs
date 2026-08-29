namespace TrueLogs.Emitter.Adapter.File.LogFileReader;

public interface ILogFileReader : IDisposable
{
    IAsyncEnumerable<string> ReadLogsAsync(CancellationToken token);
}
