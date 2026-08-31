namespace TrueLogs.Storage.Logs.Managers;

public interface ILogManager
{
    Task Add(IEnumerable<LogModel> logs, CancellationToken token);
}