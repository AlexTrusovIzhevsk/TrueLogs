namespace TrueLogs.Emitter.Adapter.File.LogFileManagers;

public interface ILogFileManager : IDisposable
{
    Task StartWatch(CancellationToken token);
}
