namespace TrueLogs.Emitter.Adapter.File.LogFileWatcherds;

public class LogFileChangeEventArgs : EventArgs
{
    public required string LogFileFullPath { get; init; }
}

public delegate Task FileLogChangeEventHandler(object sender, LogFileChangeEventArgs e);

public interface ILogFileWatcher : IDisposable
{
    event FileLogChangeEventHandler FileLogChange;
}