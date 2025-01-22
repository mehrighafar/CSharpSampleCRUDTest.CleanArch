using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace CSharpSampleCRUDTest.CleanArch.Web.Configurations;

public static class OpenTelemetryConfigs
{
  public static WebApplicationBuilder AddOpentTelemetryConfigs(
    this WebApplicationBuilder builder
    , string activitySourceName
    , string serviceName
    , string serviceVersion
    , string collectorEndpoint
    )
  {

    // Traces
    builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
      if (builder.Environment.IsDevelopment())
      {
        tracing.SetSampler<AlwaysOnSampler>();
      }
      tracing.AddSource(activitySourceName)
             .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion))
             .AddAspNetCoreInstrumentation()
             .AddHttpClientInstrumentation()
             .AddOtlpExporter(options =>
             {
               options.Endpoint = new Uri(collectorEndpoint);
               options.Protocol = OtlpExportProtocol.Grpc;
             });
    })
    // Metrics
    .WithMetrics(metrics =>
    {
      metrics.AddMeter("Microsoft.AspNetCore.Hosting"
        , "Microsoft.AspNetCore.Server.Kestrel"
        , "System.Net.Http"
        , serviceName)
             .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion))
             .AddAspNetCoreInstrumentation()
             .AddHttpClientInstrumentation()
             .AddRuntimeInstrumentation()
             .AddPrometheusExporter();
    });

    // Logging
    builder.Logging.AddOpenTelemetry(options =>
    {
      options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion));
      options.IncludeScopes = true;
      options.IncludeFormattedMessage = true;
      options.AddOtlpExporter();
    });

    return builder;
  }
}
