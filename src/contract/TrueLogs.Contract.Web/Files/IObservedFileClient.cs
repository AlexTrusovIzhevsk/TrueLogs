using TrueLogs.Contract.Web.Dto;

namespace TrueLogs.Contract.Web.Files;

public interface IObservedFileClient
{
    Task Add(AddObservedFileContract addObservedFile, CancellationToken token);
    Task Remove(RemoveObservedFileContract removeObservedFile, CancellationToken token);
    Task<AllObservedFileContract> GetAll(CancellationToken token);
}