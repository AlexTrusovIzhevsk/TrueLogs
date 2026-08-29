using Microsoft.Extensions.Logging;
using TrueLogs.TrueLang.Lexers;
using TrueLogs.TrueLang.Parsers;
using TrueLogs.TrueLang.Parsers.Nodes;

namespace TrueLogs.Storage.Logs.Providers;

public abstract class LogProvider(ILogger<LogProvider> logger)
{
    public Task<LogsModel> Get(
        GetLogsModel model,
        CancellationToken token)
    {
        try
        {
            var lexer = new Lexer(model.Query);
            var parser = new Parser(lexer);
            var ast = parser.Parse();

            var result = Get(model.Skip, model.Take, ast, token);
            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed get logs");
            throw;
        }
    }

    protected abstract Task<LogsModel> Get(
        int skip,
        int take,
        Node? node,
        CancellationToken token
    );
}