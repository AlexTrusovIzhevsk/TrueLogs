namespace TrueLogs.Emitter.Adapter.File.LogFileConfig;

public class LogFileConfigRedaer : ILogFileConfigRedaer
{
    private readonly LogFileConfig _config;

    public LogFileConfigRedaer(LogFileConfig config)
    {
        _config = config;
    }

    public LogFileConfig GetConfig()
    {
        return _config;
    }
}
