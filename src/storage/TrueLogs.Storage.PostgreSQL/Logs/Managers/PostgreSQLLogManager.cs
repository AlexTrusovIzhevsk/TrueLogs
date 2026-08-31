using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TrueLogs.Storage.Logs.Managers;

namespace TrueLogs.Storage.PostgreSQL.Logs.Managers;

public class PostgreSQLLogManager : ILogManager
{
    private readonly string _connectionString;
    private readonly ILogger<PostgreSQLLogManager> _logger;

    public PostgreSQLLogManager(string connectionString, ILogger<PostgreSQLLogManager> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task Add(IEnumerable<LogModel> logs, CancellationToken token)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO logs (
                id, 
                timestamp, 
                level, 
                messagetemplate, 
                renderedmessage,
                properties, 
                exception, 
                source, 
                environment,
                traceid, 
                spanid, 
                servicetimestamp
            ) VALUES (
                @Id,  
                @Timestamp,  
                @Level,  
                @MessageTemplate,  
                @RenderedMessage,
                @Properties,  
                @Exception,  
                @Source,  
                @Environment,  
                @TraceId,  
                @SpanId,  
                @ServiceTimestamp
            );
        ";

        await connection.ExecuteAsync(sql, logs);
    }
}
