using LiteDB.Async;
using TrueLogs.Storage.LiteDB.Logs.Mappers;
using TrueLogs.Storage.Logs.Providers;

namespace TrueLogs.Storage.LiteDB.Logs.Repositories;

public class LiteDbLogRepository
{
    private readonly LiteDatabaseAsync _db;
    private readonly ILiteCollectionAsync<LogModel> _logs;

    static LiteDbLogRepository()
    {
        LogBsonDictionaryMapper.RegisterMapping();
    }

    public LiteDbLogRepository()
    {
        var connectionString = $"Filename={Constants.DataDirPath}/{Constants.FileDbName};Connection=shared";
        _db = new LiteDatabaseAsync(connectionString);
        _logs = _db.GetCollection<LogModel>();
    }

    public async Task InsertAsync(IEnumerable<LogModel> logs, CancellationToken token)
    {
        await _logs.UpsertAsync(logs);
    }

    public async Task<LogsModel> GetAllAsync(int skip, int take, string? queryText, CancellationToken token)
    {
        var query = _logs.Query();

        if (queryText is not null)
        {
            query = query.Where(queryText);
        }

        query = query
            .OrderByDescending(l => l.Timestamp);

        var totalCount = await query
            .CountAsync();

        var logs = await query
            .Skip(skip)
            .Limit(take)
            .ToListAsync();

        return new LogsModel { 
            Logs = logs,
            TotalCount = totalCount,
        };
    }
}
