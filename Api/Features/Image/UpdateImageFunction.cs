using AutoMapper;
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
using System.Net;
using System.Text.Json;

namespace Couple.Api.Features.Image;

public class UpdateImageFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMapper _mapper;

    public UpdateImageFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
        _mapper = mapper;
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
        if (claims.PartnerId == null) return req.CreateResponse(HttpStatusCode.BadRequest);

        var dto = form.Value;
        var url = Environment.GetEnvironmentVariable("GetImageUrl");
        var now = _dateTimeService.Now;
        var contentId = $"{dto.Id}_{now.Ticks / TimeSpan.TicksPerMillisecond}";
        var toCreate = new HyperlinkChange(Guid.NewGuid(),
            Command.Update,
            claims.PartnerId,
            now,
            contentId,
            Entity.Image,
            JsonSerializer.Serialize(_mapper.Map<Model.Image>(dto)),
            url);

        _context
            .HyperlinkChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        var client = new BlobClient(connectionString, "images", contentId);
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
                if (!ImageExtensions.IsImage(new MemoryStream(data))) context.AddFailure("Invalid file type");
            });
        }
    }
}
