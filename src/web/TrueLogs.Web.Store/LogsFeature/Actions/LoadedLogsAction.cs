using TrueLogs.Web.Store.LogsFeature.States;

namespace TrueLogs.Web.Store.LogsFeature.Actions;

public record LoadedLogsAction(
    List<Log>? Logs,
    string? Error,
    int TotalCount,
    int Skip,
    int Take
);
