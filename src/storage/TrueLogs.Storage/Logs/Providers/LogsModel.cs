namespace TrueLogs.Storage.Logs.Providers;

public class LogsModel
{
    public required IEnumerable<LogModel> Logs { get; init; }
    public required int TotalCount { get; init; }

}