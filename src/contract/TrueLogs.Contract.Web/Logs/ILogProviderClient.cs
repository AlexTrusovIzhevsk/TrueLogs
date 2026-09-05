using TrueLogs.Contract.Web.Logs.Dto;
namespace TrueLogs.Contract.Web.Logs;

public interface ILogProviderClient
{
    Task<LogsWebContract> Get(int skip, int take, string query, CancellationToken token);
}