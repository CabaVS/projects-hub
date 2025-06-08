using CabaVS.ExpenseTracker.API.Logging;
using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Infrastructure;
using CabaVS.ExpenseTracker.Infrastructure.Configuration.FromAzure;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;
using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

builder.Configuration.AddJsonStreamFromBlob(isDevelopment);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddScoped<UserIdEnrichmentMiddleware>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(isDevelopment);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration, isDevelopment);

// Only for Aspire (DEV only)
if (isDevelopment)
{
    builder.AddServiceDefaults();
}

WebApplication app = builder.Build();

// Only for Aspire (DEV only)
if (isDevelopment)
{
    app.MapDefaultEndpoints();
}

app.UseAuthentication();
app.UseAuthorization();
        
app.UseMiddleware<UserIdEnrichmentMiddleware>();
        
app.UseFastEndpoints();
        
if (isDevelopment)
{
    app.UseSwaggerGen();
}

await app.RunAsync();
