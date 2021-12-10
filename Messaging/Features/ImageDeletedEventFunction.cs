using Azure.Storage.Blobs;
using Couple.Messaging.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;

namespace Couple.Messaging.Features;

public class ImageDeletedEventFunction
{
    [FunctionName("ImageDeletedEventFunction")]
    public async Task Run([EventGridTrigger] Event @event)
    {
        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        var client = new BlobClient(connectionString, "images", @event.Subject);
        await client.DeleteIfExistsAsync();
    }
}
