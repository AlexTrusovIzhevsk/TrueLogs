using Microsoft.AspNetCore.Mvc;
using TrueLogs.Api.Provider.Host.Mappers;
using TrueLogs.Contract.Web.Logs;
using TrueLogs.Contract.Web.Logs.Dto;
using TrueLogs.Storage.Logs.Providers;

namespace TrueLogs.Api.Provider.Host.Controllers;

[ApiController]
[Route(LogProviderWebEndpoints.LogProviderRoute)]
public class LogProviderController(
    LogProvider _provider
    ) : ControllerBase, ILogProviderClient
{
    [HttpGet(LogProviderWebEndpoints.LogProviderGetAllRoute)]
    public async Task<LogsWebContract> Get(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromQuery] string query = "",
        CancellationToken token = default)
    {
        var getLogsModel = new GetLogsModel(skip, take, query);
        var models = await _provider.Get(getLogsModel, token);
        var dtos = models.Map();
        return dtos;
    }
}