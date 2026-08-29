namespace TrueLogs.Api.Emitter.Host.Configurations;

public static class AspireExtentions
{
    public static WebApplicationBuilder AddAspire(this WebApplicationBuilder builder) 
    {
        return builder.AddServiceDefaults();
    }

    public static WebApplication AddAspire(this WebApplication application)
    {
        return application.MapDefaultEndpoints();
    }
}
