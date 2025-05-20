using System.Text;
using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CabaVS.ExpenseTracker.Infrastructure.Configuration.FromAzure;

public static class AzureBlobJsonConfigurationProvider
{
    public static Stream JsonStream()
    {
        var uri = new Uri(
            Environment.GetEnvironmentVariable("CVS_CONFIGURATION_FROM_AZURE_URL")
            ?? throw new InvalidOperationException("Configuration URL is not provided."));

        var blobClient = new BlobClient(uri, new DefaultAzureCredential());

        Response<BlobDownloadResult>? response = blobClient.DownloadContent();

        var json = response.Value.Content.ToString();

        return new MemoryStream(Encoding.UTF8.GetBytes(json));
    }
}
