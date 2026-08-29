namespace TrueLogs.Emitter.Adapter.File.PositionTrackers;

public interface IPositionTracker
{
    Task<long> GetCurrentPositionAsync(CancellationToken token);
    Task SetCurrentPositionAsync(long position, CancellationToken token);

}
