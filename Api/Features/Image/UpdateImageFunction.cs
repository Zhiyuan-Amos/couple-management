using System.Net;
using Azure.Storage.Blobs;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared.Model;
using Couple.Shared.Model.Image;
using Couple.Shared.Utility;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Couple.Api.Features.Image;

public class UpdateImageFunction
{
    private readonly ChangeContext _changeContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private readonly ImageContext _imageContext;

    public UpdateImageFunction(ChangeContext changeContext,
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        ImageContext imageContext)
    {
        _changeContext = changeContext;
        _imageContext = imageContext;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("UpdateImageFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Images")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var form = await req.GetJsonBody<UpdateImageDto, Validator>();

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
            Command.Update,
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
        await client.UploadAsync(new BinaryData(dto.Data), true);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private class Validator : AbstractValidator<UpdateImageDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.TakenOn).NotEmpty();
            RuleFor(dto => dto.Data).Custom((data, context) =>
            {
                if (!ImageExtensions.IsImage(new MemoryStream(data)))
                {
                    context.AddFailure("Invalid file type");
                }
            });
        }
    }
}
