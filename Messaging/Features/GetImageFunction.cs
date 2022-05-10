using Couple.Messaging.Data;
using Couple.Messaging.Model;
using Couple.Shared.Model;

namespace Couple.Messaging.Features;

// File is part of Messaging rather than Api so it doesn't interfere with Api's auth
public class GetImageFunction
{
    private readonly ImageContext _context;

    public GetImageFunction(ImageContext context) => _context = context;

    [FunctionName("GetImageFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Images")]
        HttpRequest req,
        ILogger log)
    {
        var form = await req.GetJsonBody<List<string>, Validator>();

        if (!form.IsValid)
        {
            log.LogWarning("{ErrorMessage}", form.ErrorMessage());
            return form.ToBadRequest();
        }

        var model = form.Value;

        var imageIdToImage = await _context.Images
            .Where(image => model.Contains(image.TimeSensitiveId))
            .ToDictionaryAsync(image => image.TimeSensitiveId, image => image);

        if (model.Count != imageIdToImage.Count)
        {
            return new InternalServerErrorResult();
        }

        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        List<HyperlinkContent> toReturn = new();
        foreach (var id in model)
        {
            var client = new BlobClient(connectionString, "images", id);
            var stream = new MemoryStream();
            await client.DownloadToAsync(stream);
            var data = stream.ToArray();
            await stream.ReadAsync(data.AsMemory(0, (int)stream.Length));

            var image = imageIdToImage[id];
            var content = new ImageDto(image.ObjectId, image.TakenOn, data, image.IsFavourite);
            toReturn.Add(new(id, content));
        }

        return new OkObjectResult(toReturn);
    }

    public class Validator : AbstractValidator<List<string>>
    {
        public Validator() => RuleFor(dto => dto).NotEmpty();
    }
}
