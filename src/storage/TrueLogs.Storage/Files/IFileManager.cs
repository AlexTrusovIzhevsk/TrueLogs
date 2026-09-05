namespace TrueLogs.Storage.Files;

public interface IFileManager
{
    Task Add(string path, CancellationToken token);
    Task Remove(string path, CancellationToken token);
    Task<IEnumerable<string>> GetAll(CancellationToken token);
}