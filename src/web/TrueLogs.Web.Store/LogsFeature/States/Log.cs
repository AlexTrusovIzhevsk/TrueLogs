namespace TrueLogs.Web.Store.LogsFeature.States;

public record Log(
    DateTime Timestamp,
    Level Level,
    string MessageTemplate,
    string RenderedMessage,
    Dictionary<string, object?> Properties,
    string? Exception,
    string Source,
    string? Environment,
    string? TraceId,
    string? SpanId
    );