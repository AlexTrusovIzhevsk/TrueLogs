namespace TrueLogs.Storage.Logs;

public class LogModel
{
    public Guid Id = Guid.NewGuid();
    public DateTime ServiceTimestamp { get; init; } = DateTime.UtcNow;

    public required DateTime Timestamp { get; init; }
    public required Level Level { get; init; }
    public required string MessageTemplate { get; init; }
    public required string RenderedMessage { get; init; }
    public required Dictionary<string, object?> Properties { get; init; }
    public required string? Exception { get; init; }
    public required string Source { get; init; }
    public required string? Environment { get; init; }
    public required string? TraceId { get; init; }
    public required string? SpanId { get; init; }

}