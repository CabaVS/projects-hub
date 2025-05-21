using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, bool isDevelopment)
    {
        if (!isDevelopment)
        {
            services.AddOpenTelemetry().UseAzureMonitor();
        }
        
        return services;
    }
}
