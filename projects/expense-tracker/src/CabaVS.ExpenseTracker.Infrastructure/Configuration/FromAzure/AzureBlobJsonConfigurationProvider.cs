using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace CabaVS.ExpenseTracker.Infrastructure.Configuration.FromAzure;

public static class AzureBlobJsonConfigurationProvider
{
    public static IConfigurationBuilder AddJsonStreamFromBlob(this IConfigurationBuilder builder, string envName = "CVS_CONFIGURATION_FROM_AZURE_URL")
    {
        var configUrl = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrWhiteSpace(configUrl))
        {
            return builder;
        }
        
        var blobClient = new BlobClient(new Uri(configUrl), new DefaultAzureCredential());

        Response<BlobDownloadResult>? response = blobClient.DownloadContent();

        var stream = response.Value.Content.ToStream();
        return builder.AddJsonStream(stream);
    }
}
