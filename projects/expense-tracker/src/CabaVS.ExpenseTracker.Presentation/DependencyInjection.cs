using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using CabaVS.ExpenseTracker.Presentation.Logging;
using CabaVS.ExpenseTracker.Presentation.UserContext;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigureHostBuilder hostBuilder,
        IConfiguration configuration,
        bool isDevelopment = false)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        hostBuilder.UseSerilog();
        
        services.AddScoped<UserIdEnrichmentMiddleware>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Audience = configuration["Authentication:Audience"];
                options.Authority = configuration["Authentication:Authority"];
                options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"] ?? bool.TrueString);

                options.TokenValidationParameters = new TokenValidationParameters { RoleClaimType = "realm_access.roles" };
            });
        services.AddAuthorization();
        
        services.AddScoped<ICurrentUserAccessor, DummyCurrentUserAccessor>();
        
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

    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseMiddleware<UserIdEnrichmentMiddleware>();
        
        app.UseFastEndpoints();
        
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwaggerGen();
        
        return app;
    }
}
