namespace TrueLogs.Contract.Web.Logs;

public static class LogProviderWebEndpoints
{
    public const string LogProviderRoute = "api/log-provider";

    public const string LogProviderGetAllRoute = "logs";

    public const string LogProviderGetAllEndpoint = $"{LogProviderRoute}/{LogProviderGetAllRoute}?skip=$skip&take=$take&query=$query";

}
