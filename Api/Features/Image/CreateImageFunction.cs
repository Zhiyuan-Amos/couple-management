using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared;
using Couple.Shared.Model;
using Couple.Shared.Model.Image;
using Couple.Shared.Utility;

namespace Couple.Api.Features.Image;

public class CreateImageFunction
{
    private readonly ChangeContext _changeContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private readonly ImageContext _imageContext;

    public CreateImageFunction(ChangeContext changeContext,
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        ImageContext imageContext)
    {
        _changeContext = changeContext;
        _currentUserService = currentUserService;
        _dateTimeService = dateTimeService;
        _imageContext = imageContext;
    }

    [Function("CreateImageFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Images")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var form = await req.GetJsonBody<CreateImageDto, Validator>();

        if (!form.IsValid)
        {
            var logger = executionContext.GetLogger(GetType().Name);
            var errorMessage = form.ErrorMessage();
            logger.LogWarning("{ErrorMessage}", errorMessage);
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync(errorMessage);
            return response;
        }

        var claims = _currentUserService.GetClaims(req.Headers);

        var dto = form.Value;

        var imageToCreate = new Model.Image(dto.Id, dto.TakenOn, dto.IsFavourite);
        _imageContext
            .Images
            .Add(imageToCreate);
        await _imageContext.SaveChangesAsync();

        var url = Environment.GetEnvironmentVariable("GetImageUrl")!;
        var toCreate = new HyperlinkChange(Guid.NewGuid(),
            Command.Create,
            claims.PartnerId,
            _dateTimeService.Now,
            imageToCreate.TimeSensitiveId,
            Entity.Image,
            url);

        _changeContext
            .HyperlinkChanges
            .Add(toCreate);
        await _changeContext.SaveChangesAsync();

        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        var client = new BlobClient(connectionString, "images", imageToCreate.TimeSensitiveId);
        await client.UploadAsync(new BinaryData(dto.Data));

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private class Validator : AbstractValidator<CreateImageDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.TakenOn).NotEmpty();
            RuleFor(dto => dto.Data).Custom((data, context) =>
            {
                if (data == null)
                {
                    context.AddFailure("Data cannot be null");
                    return;
                }

                if (!ImageExtensions.IsImage(new MemoryStream(data))) context.AddFailure("Invalid file type");

                if (data.Length > Constants.MaxFileSize) context.AddFailure("File exceeded size limit");
            });
        }
    }
}