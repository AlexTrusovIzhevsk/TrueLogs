using TrueLogs.Storage.LiteDB.Logs.Repositories;
using TrueLogs.Storage.Logs.Managers;

namespace TrueLogs.Storage.LiteDB.Logs.Managers;

public class LiteDBLogManager(LiteDbLogRepository _liteDbLogRepository) : ILogManager
{
    public Task Add(IEnumerable<LogModel> logs)
    {
        return _liteDbLogRepository.InsertAsync(logs);
    }
}
