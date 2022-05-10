using Azure.Storage.Blobs;
using Couple.Messaging.Data;
using Couple.Messaging.Model;
using FluentValidation;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Couple.Messaging.Features;

public class ImageDeletedEventFunction
{
    private readonly ImageContext _context;

    public ImageDeletedEventFunction(ImageContext context) => _context = context;

    [FunctionName("ImageDeletedEventFunction")]
    public async Task Run([EventGridTrigger] Event @event, ILogger log)
    {
        var validationResult = new Validator().Validate(@event);
        if (!validationResult.IsValid)
        {
            var errorMessage = validationResult.Errors
                .Select(error => error.ToString())
                .ToString();
            log.LogWarning("{ErrorMessage}", errorMessage);
            return;
        }

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

    public class Validator : AbstractValidator<Event>
    {
        public Validator() => RuleFor(dto => dto.Subject).NotEmpty();
    }
}