using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CabaVS.ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, bool isDevelopment)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(_ => ResourceBuilder.CreateDefault())
            .WithMetrics(metrics =>
            {
                MeterProviderBuilder builder = metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddSqlClientInstrumentation();
                if (isDevelopment)
                {
                    builder.AddOtlpExporter();
                }
                else
                {
                    builder.AddAzureMonitorMetricExporter();
                }
            })
            .WithTracing(tracing =>
            {
                TracerProviderBuilder builder = tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation();
                if (isDevelopment)
                {
                    builder.AddOtlpExporter();
                }
                else
                {
                    builder.AddAzureMonitorTraceExporter();
                }
            });
        
        return services;
    }
}
