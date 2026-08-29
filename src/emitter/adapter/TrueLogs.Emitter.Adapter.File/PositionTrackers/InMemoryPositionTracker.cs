
namespace TrueLogs.Emitter.Adapter.File.PositionTrackers;

public class InMemoryPositionTracker : IPositionTracker
{
    private long _currentPosition = 0;

    public Task<long> GetCurrentPositionAsync(CancellationToken token)
    {
        var result = Interlocked.Read(ref _currentPosition);
        return Task.FromResult(result);
    }

    public Task SetCurrentPositionAsync(long position, CancellationToken token)
    {
        Interlocked.Exchange(ref _currentPosition, position);
        return Task.CompletedTask;
    }
}
