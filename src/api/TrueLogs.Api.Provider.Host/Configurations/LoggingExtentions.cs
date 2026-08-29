using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.OpenTelemetry;
using TrueLogs.Storage.LiteDB.Logs;

namespace TrueLogs.Api.Provider.Host.Configurations;

public static class LoggingExtentions
{
    internal const string LogDir = "logs";
    internal const string ServiceName = "provider";

    internal const string ProviderRowPath = $"{Constants.DataDirPath}/{LogDir}/{ServiceName}/{ServiceName}.row.log";
    internal const string ProviderStructuredPath = $"{Constants.DataDirPath}/{LogDir}/{ServiceName}/{ServiceName}.structured.log";

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) => config
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(ProviderRowPath)
            .WriteTo.File(new CompactJsonFormatter(), ProviderStructuredPath)
            .WriteTo.Seq("http://localhost:5341")
            .WriteTo.OpenTelemetry(options => WriteToAspire(options, builder)
        ));

        return builder;
    }

    private static void WriteToAspire(BatchedOpenTelemetrySinkOptions options, WebApplicationBuilder builder)
    {
        // Aspire автоматически задает этот эндпоинт
        const string aspireDefaultLogEndpoint = "OTEL_EXPORTER_OTLP_ENDPOINT";
        options.Endpoint = builder.Configuration[aspireDefaultLogEndpoint];
        options.Protocol = OtlpProtocol.Grpc;
        options.IncludedData = IncludedData.TraceIdField | IncludedData.SpanIdField;
    }
}
