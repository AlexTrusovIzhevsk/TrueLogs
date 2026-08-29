namespace TrueLogs.Api.Host.Configurations;

public static class CorsExtentions
{
    public const string DevelopmentPolicyCorsName = "DevelopmentPolicy";

    public static WebApplicationBuilder AddDevelopmentPolicyCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options
            .AddPolicy(DevelopmentPolicyCorsName, policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            )
        );

        return builder;
    }
}
