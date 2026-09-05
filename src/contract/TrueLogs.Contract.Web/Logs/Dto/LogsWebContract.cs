namespace TrueLogs.Contract.Web.Logs.Dto;

public class LogsWebContract
{
    public IEnumerable<LogWebContract> Logs { get; set; }
    public int TotalCount { get; set; }
}
