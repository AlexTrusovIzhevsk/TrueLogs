namespace TrueLogs.Web.Store.LogsFeature.Actions;

public record LoadLogsAction(
    int TotalCount,
    int Skip,
    int Take,
    string Query
);