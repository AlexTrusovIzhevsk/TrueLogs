using TrueLogs.Contract.Web.Enums;

namespace TrueLogs.Contract.Web.Dto;

public class LogWebContract
{
    public DateTime Timestamp { get; set; }
    public LevelWebContract Level { get; set; }
    public string MessageTemplate { get; set; }
    public string RenderedMessage { get; set; }
    public Dictionary<string, object?> Properties { get; set; }
    public string? Exception { get; set; }
    public string Source { get; set; }
    public string? Environment { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
}
