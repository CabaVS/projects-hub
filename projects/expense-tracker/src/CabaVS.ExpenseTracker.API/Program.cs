using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Infrastructure;
using CabaVS.ExpenseTracker.Infrastructure.Configuration.FromAzure;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

var loadAzureConfiguration = bool.Parse(Environment.GetEnvironmentVariable("CVS_CONFIGURATION_FROM_AZURE") ?? bool.FalseString);
if (loadAzureConfiguration)
{
    builder.Configuration.AddJsonStream(AzureBlobJsonConfigurationProvider.JsonStream());
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure(isDevelopment);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPresentation(builder.Host, builder.Configuration, isDevelopment);

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

app.UsePresentation();

await app.RunAsync();
