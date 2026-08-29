using Fluxor;

namespace TrueLogs.Web.Store.LogsFeature.States;

[FeatureState]
public record LogsFeatureState
{
    public List<Log> Logs { get; init; } = [];
    public bool IsLoading { get; init; } = false;
    public string? Error { get; init; } = null;
    public int TotalCount { get; init; }
    public int Skip { get; init; }
    public int Take { get; init; }
    public string Query { get; init; } = string.Empty;

    public LogsFeatureState(List<Log> logs, bool isLoading, string? error)
    {
        Logs = logs;
        IsLoading = isLoading;
        Error = error;
    }

    public LogsFeatureState() { }
}