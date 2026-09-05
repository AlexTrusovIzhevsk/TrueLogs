using TrueLogs.Contract.Web.Logs.Dto;
using TrueLogs.Contract.Web.Logs.Enums;
using TrueLogs.Web.Store.LogsFeature.States;

namespace TrueLogs.Api.Mappers;

internal static class LogContractMapExtention
{
    internal static IEnumerable<Log> Map(this IEnumerable<LogWebContract> logs)
        => logs.Select(Map);

    internal static Log Map(this LogWebContract log)
        => new Log(
            Timestamp: log.Timestamp,
            Level: log.Level.Map(),
            MessageTemplate: log.MessageTemplate,
            RenderedMessage: log.RenderedMessage,
            Properties: log.Properties,
            Exception: log.Exception,
            Source: log.Source,
            Environment: log.Environment,
            TraceId: log.TraceId,
            SpanId: log.SpanId
        );

    internal static Level Map(this LevelWebContract level)
        => level switch
        {
            LevelWebContract.Trace => Level.Trace,
            LevelWebContract.Debug => Level.Debug,
            LevelWebContract.Information => Level.Information,
            LevelWebContract.Warning => Level.Warning,
            LevelWebContract.Error => Level.Error,
            LevelWebContract.Fatal => Level.Fatal,
            _ => throw new NotImplementedException(level.ToString())
        };
}