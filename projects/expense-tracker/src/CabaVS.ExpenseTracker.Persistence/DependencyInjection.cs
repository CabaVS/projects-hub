using Azure.Core;
using Azure.Identity;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Persistence.ReadRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlDatabase")
                               ?? throw new InvalidOperationException("Connection string to the database is not configured.");
        
        services.AddTransient(_ =>
        {
            var useEntraId = bool.Parse(configuration["CVS_PERSISTENCE_AUTH_ENTRA_ID"] ?? "true");
            if (!useEntraId)
            {
                return new SqlConnection(connectionString);
            }

            var tokenRequest = new TokenRequestContext(["https://database.windows.net/.default"]);
            var token = new DefaultAzureCredential().GetToken(tokenRequest).Token;
                
            return new SqlConnection(connectionString) { AccessToken = token };

        });
        
        services.AddDbContext<ApplicationDbContext>(
            (sp, options) =>
            {
                SqlConnection sqlConnection = sp.GetRequiredService<SqlConnection>();
                options.UseSqlServer(sqlConnection, sqlOptions => sqlOptions.EnableRetryOnFailure(5));
            },
            contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Scoped);

        // Note: While Transaction lifetime does enforce true isolation of UoW, it's recommended by MS to use Scoped lifetime for web applications
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddSingleton<IWorkspaceReadRepository, WorkspaceReadRepository>();
        
        return services;
    }
}
