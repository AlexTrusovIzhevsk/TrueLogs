using Microsoft.AspNetCore.Mvc;
using TrueLogs.Api.Provider.Host.Mappers;
using TrueLogs.Contract.Clients;
using TrueLogs.Contract.Web;
using TrueLogs.Contract.Web.Dto;
using TrueLogs.Storage.Logs.Providers;

namespace TrueLogs.Api.Provider.Host.Controllers;

[ApiController]
[Route(WebEndpoints.LogProviderRoute)]
public class LogProviderController(
    LogProvider _provider
    ) : ControllerBase, ILogProviderClient
{
    [HttpGet(WebEndpoints.LogProviderGetAllRoute)]
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