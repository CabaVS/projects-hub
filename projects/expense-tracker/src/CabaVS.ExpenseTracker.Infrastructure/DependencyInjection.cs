using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CabaVS.ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ConfigureOpenTelemetry(services);
        
        return services;
    }

    private static void ConfigureOpenTelemetry(IServiceCollection services) =>
        services.AddOpenTelemetry()
            .ConfigureResource(_ => ResourceBuilder.CreateDefault())
            .WithMetrics(metrics =>
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter())
            .WithTracing(tracing =>
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter());
}
