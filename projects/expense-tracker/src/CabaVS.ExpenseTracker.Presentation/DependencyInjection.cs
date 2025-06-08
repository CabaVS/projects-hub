using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using CabaVS.ExpenseTracker.Presentation.Authentication;
using CabaVS.ExpenseTracker.Presentation.UserContext;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment = false)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Audience = configuration["Authentication:Audience"];
                options.Authority = configuration["Authentication:Authority"];
                options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"] ?? bool.TrueString);
            });
        services.AddAuthorization();

        services.AddScoped<IClaimsTransformation, KeycloakRoleClaimsTransformer>();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        services.AddFastEndpoints();
        
        if (!isDevelopment)
        {
            return services;
        }
        
        services.SwaggerDocument(x => 
        {
            x.AutoTagPathSegmentIndex = 0;
            x.EnableJWTBearerAuth = true;
            
            x.DocumentSettings = s =>
            {
                s.Title = "Expense Tracker API";
                s.Version = "v1";
            }; 
        });
        
        return services;
    }
}
