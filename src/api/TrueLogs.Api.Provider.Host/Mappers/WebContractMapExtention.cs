using TrueLogs.Contract.Web.Logs.Dto;
using TrueLogs.Contract.Web.Logs.Enums;
using TrueLogs.Storage.Logs;
using TrueLogs.Storage.Logs.Providers;

namespace TrueLogs.Api.Provider.Host.Mappers;

internal static class WebContractMapExtention
{
    internal static LogsWebContract Map(this LogsModel model)
        => new()
        {
            Logs = model.Logs.Map(),
            TotalCount = model.TotalCount,
        };

    internal static IEnumerable<LogWebContract> Map(this IEnumerable<LogModel> logs)
        => logs.Select(Map);

    internal static LogWebContract Map(this LogModel logDto)
        => new()
        {
            Timestamp = logDto.Timestamp,
            Level = logDto.Level.Map(),
            MessageTemplate = logDto.MessageTemplate,
            RenderedMessage = logDto.RenderedMessage,
            Properties = logDto.Properties,
            Exception = logDto.Exception,
            Source = logDto.Source,
            Environment = logDto.Environment,
            TraceId = logDto.TraceId,
            SpanId = logDto.SpanId,
        };

    internal static LevelWebContract Map(this Level level)
        => level switch
        {
            Level.Trace => LevelWebContract.Trace,
            Level.Debug => LevelWebContract.Debug,
            Level.Information => LevelWebContract.Information,
            Level.Warning => LevelWebContract.Warning,
            Level.Error => LevelWebContract.Error,
            Level.Fatal => LevelWebContract.Fatal,
            _ => throw new NotImplementedException(level.ToString())
        };
}