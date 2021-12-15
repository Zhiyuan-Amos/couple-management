using Azure.Storage.Blobs;
using Couple.Messaging.Data;
using Couple.Messaging.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.EntityFrameworkCore;

namespace Couple.Messaging.Features;

public class ImageDeletedEventFunction
{
    private readonly ImageContext _context;

    public ImageDeletedEventFunction(ImageContext context)
    {
        _context = context;
    }

    [FunctionName("ImageDeletedEventFunction")]
    public async Task Run([EventGridTrigger] Event @event)
    {
        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        var client = new BlobClient(connectionString, "images", @event.Subject);
        await client.DeleteIfExistsAsync();

        var toDelete = await _context.Images
            .SingleOrDefaultAsync(image => image.TimeSensitiveId == @event.Subject);

        if (toDelete != null)
        {
            _context.Images
                .Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}
