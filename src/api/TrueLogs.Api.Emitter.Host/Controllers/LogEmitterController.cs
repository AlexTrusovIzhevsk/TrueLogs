using Microsoft.AspNetCore.Mvc;
using TrueLogs.Api.Emitter.Host.Mappers;
using TrueLogs.Contract.Clients;
using TrueLogs.Contract.Emitter;
using TrueLogs.Contract.Emitter.Dto;
using TrueLogs.Storage.Logs.Managers;

namespace TrueLogs.Api.Emitter.Host.Controllers;

[ApiController]
[Route(EmitterEndpoints.LogEmitterRoute)]
public class LogEmitterController(
    ILogManager _manager
    ) : ControllerBase, ILogEmitterClient
{
    [HttpPost]
    public async Task Emit(IEnumerable<LogEmitterContract> logs, CancellationToken token)
    {
        var models = logs.Map();

        await _manager.Add(models);
    }
}