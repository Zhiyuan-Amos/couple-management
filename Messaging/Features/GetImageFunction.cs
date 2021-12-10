using Azure.Storage.Blobs;
using Couple.Messaging.Infrastructure;
using Couple.Shared.Model.Image;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Couple.Messaging.Features;

// File is part of Messaging rather than Api so it doesn't interfere with Static Web App's auth
public class GetImageFunction
{
    [FunctionName("GetImageFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Images")]
        HttpRequest req, ILogger log)
    {
        var form = await req.GetJsonBody<List<Guid>, Validator>();

        if (!form.IsValid)
        {
            log.LogWarning("{ErrorMessage}", form.ErrorMessage());
            return form.ToBadRequest();
        }

        var model = form.Value;

        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        List<ImageDto> toReturn = new();
        foreach (var id in model)
        {
            var client = new BlobClient(connectionString, "images", id.ToString());
            var stream = new MemoryStream();
            await client.DownloadToAsync(stream);
            var data = stream.ToArray();
            await stream.ReadAsync(data.AsMemory(0, (int)stream.Length));

            toReturn.Add(new(id, data));
        }

        return new OkObjectResult(toReturn);
    }

    public class Validator : AbstractValidator<List<Guid>>
    {
        public Validator()
        {
            RuleFor(dto => dto).NotEmpty();
        }
    }
}
