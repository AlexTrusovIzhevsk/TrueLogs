using Microsoft.AspNetCore.Mvc;
using TrueLogs.Contract.Web.Dto;
using TrueLogs.Contract.Web.Files;
using TrueLogs.Storage.Files;

namespace TrueLogs.Api.Provider.Host.Controllers;

[ApiController]
[Route(ObservedFileWebEndpoints.ObservedFileWebRoute)]
public class ObservedFileController(
    IFileManager _manager
    ) : ControllerBase, IObservedFileClient
{
    [HttpPost]
    public async Task Add(
        [FromBody] AddObservedFileContract addObservedFile, 
        CancellationToken token = default)
    {
        await _manager.Add(addObservedFile.Path, token);
    }

    [HttpDelete]
    public async Task Remove(
        [FromBody] RemoveObservedFileContract removeObservedFile, 
        CancellationToken token = default)
    {
        await _manager.Remove(removeObservedFile.Path, token);
    }

    [HttpGet]
    public async Task<AllObservedFileContract> GetAll(CancellationToken token = default)
    {
        var paths = await _manager.GetAll(token);
        var result = new AllObservedFileContract()
        {
            Paths = paths,
        };
        return result;
    }
}