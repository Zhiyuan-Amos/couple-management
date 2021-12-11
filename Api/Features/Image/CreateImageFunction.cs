using AutoMapper;
using Azure.Storage.Blobs;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared;
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

public class CreateImageFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMapper _mapper;

    public CreateImageFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
        _mapper = mapper;
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
        if (claims.PartnerId == null) return req.CreateResponse(HttpStatusCode.BadRequest);

        var dto = form.Value;
        var url = Environment.GetEnvironmentVariable("GetImageUrl");
        var toCreate = new HyperlinkChange(Guid.NewGuid(),
            Command.Create,
            claims.PartnerId,
            _dateTimeService.Now,
            dto.Id,
            Entity.Image,
            JsonSerializer.Serialize(_mapper.Map<Model.Image>(dto)),
            url);

        _context
            .HyperlinkChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
        var client = new BlobClient(connectionString, "images", dto.Id.ToString());
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
