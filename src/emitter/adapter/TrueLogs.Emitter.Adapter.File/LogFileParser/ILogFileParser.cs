using TrueLogs.Contract.Emitter.Dto;

namespace TrueLogs.Emitter.Adapter.File.LogFileParser;

public interface ILogFileParser
{
    Task<LogEmitterContract> Parse(string rowLog, CancellationToken token);
}
