using TrueLogs.Contract.Emitter.Enums;

namespace TrueLogs.Contract.Emitter.Dto;

public class LogEmitterContract
{
    public DateTime Timestamp { get; set; }
    public LevelEmitterContract Level { get; set; }
    public string MessageTemplate { get; set; }
    public string RenderedMessage { get; set; }
    public Dictionary<string, object?> Properties { get; set; }
    public string? Exception { get; set; }
    public string Source { get; set; }
    public string? Environment { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
}
