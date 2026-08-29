using TrueLogs.Contract.Web.Dto;

namespace TrueLogs.Contract.Clients;

public interface ILogProviderClient
{
    Task<LogsWebContract> Get(int skip, int take, string query, CancellationToken token);
}