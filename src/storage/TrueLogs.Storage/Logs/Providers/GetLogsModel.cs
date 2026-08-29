namespace TrueLogs.Storage.Logs.Providers;

public record GetLogsModel(
    int Skip,
    int Take,
    string Query
);