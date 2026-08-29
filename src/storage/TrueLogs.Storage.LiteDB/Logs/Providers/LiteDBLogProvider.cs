using Microsoft.Extensions.Logging;
using TrueLogs.Storage.LiteDB.Logs.Repositories;
using TrueLogs.Storage.Logs.Providers;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Translator.LiteDB;

namespace TrueLogs.Storage.LiteDB;

public class LiteDBLogProvider(LiteDbLogRepository liteDbLogRepository, ILogger<LogProvider> logger) : LogProvider(logger)
{
    protected override Task<LogsModel> Get(int skip, int take, Node? node, CancellationToken token)
    {
        string qwery = null;
        if (node != null)
        {
            var translator = new LiteDBTranslator();
            qwery = translator.Translate(node);
        }
        return liteDbLogRepository.GetAllAsync(skip, take, qwery, token);
    }
}
