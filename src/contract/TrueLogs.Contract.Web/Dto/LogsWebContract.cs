namespace TrueLogs.Contract.Web.Dto;

public class LogsWebContract
{
    public IEnumerable<LogWebContract> Logs { get; set; }
    public int TotalCount { get; set; }
}
