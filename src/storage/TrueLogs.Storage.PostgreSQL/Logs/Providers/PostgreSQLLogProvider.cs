using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TrueLogs.Storage.Logs;
using TrueLogs.Storage.Logs.Providers;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Translator.PostgreSQL;

namespace TrueLogs.Storage.LiteDB;

public class PostgreSQLLogProvider : LogProvider
{
    private readonly string _connectionString;
    private readonly PostgreSQLTranslator _translator = new();

    public PostgreSQLLogProvider(string connectionString, ILogger<PostgreSQLLogProvider> logger)
        : base(logger)
    {
        _connectionString = connectionString;
    }

    protected override async Task<LogsModel> Get(int skip, int take, Node? node, CancellationToken token)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(token);

        var whereClause = node != null ? $"WHERE {_translator.Translate(node)}" : string.Empty;

        var sql = $@"
            SELECT 
                id as Id,
                timestamp as Timestamp,
                level as Level,
                messagetemplate as MessageTemplate,
                renderedmessage as RenderedMessage,
                properties as Properties,
                exception as Exception,
                source as Source,
                environment as Environment,
                traceid as TraceId,
                spanid as SpanId,
                servicetimestamp as ServiceTimestamp
            FROM logs
            {whereClause}
            ORDER BY timestamp DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
        ";

        var countSql = $@"
            SELECT COUNT(*)
            FROM logs
            {whereClause};
        ";

        var parameters = new { Skip = skip, Take = take };

        var totalCountTask = connection.ExecuteScalarAsync<int>(countSql, parameters);
        var logsTask = connection.QueryAsync<LogModel>(sql, parameters);

        Task.WaitAll(totalCountTask, logsTask);

        return new LogsModel
        {
            Logs = logsTask.Result,
            TotalCount = totalCountTask.Result
        };
    }
}
