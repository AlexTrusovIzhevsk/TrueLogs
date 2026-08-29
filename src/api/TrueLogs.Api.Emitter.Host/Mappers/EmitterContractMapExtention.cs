using TrueLogs.Contract.Emitter.Dto;
using TrueLogs.Contract.Emitter.Enums;
using TrueLogs.Storage;

namespace TrueLogs.Api.Emitter.Host.Mappers;

internal static class EmitterContractMapExtention
{
    internal static IEnumerable<LogModel> Map(this IEnumerable<LogEmitterContract> logs)
        => logs.Select(Map);

    internal static LogModel Map(this LogEmitterContract logDto)
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


    internal static Level Map(this LevelEmitterContract level)
        => level switch
        {
            LevelEmitterContract.Trace => Level.Trace,
            LevelEmitterContract.Debug => Level.Debug,
            LevelEmitterContract.Information => Level.Information,
            LevelEmitterContract.Warning => Level.Warning,
            LevelEmitterContract.Error => Level.Error,
            LevelEmitterContract.Fatal => Level.Fatal,
            _ => throw new NotImplementedException(level.ToString())
        };
}