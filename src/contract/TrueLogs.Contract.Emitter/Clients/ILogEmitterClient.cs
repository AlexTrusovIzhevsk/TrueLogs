
using TrueLogs.Contract.Emitter.Dto;

namespace TrueLogs.Contract.Clients;

public interface ILogEmitterClient
{
    Task Emit(IEnumerable<LogEmitterContract> logs, CancellationToken token);
}