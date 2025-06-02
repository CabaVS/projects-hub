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
        services.AddTransient(_ =>
        {
            var connectionString = configuration.GetConnectionString("SqlDatabase");
            return new SqlConnection(connectionString);
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
