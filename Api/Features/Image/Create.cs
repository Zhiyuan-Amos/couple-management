using Ardalis.ApiEndpoints;
using Azure.Storage.Blobs;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Image;
using Microsoft.AspNetCore.Mvc;

namespace Couple.Api.Features.Image;

public class Create : EndpointBaseAsync
    .WithRequest<CreateImageDto>
    .WithActionResult
{
    private readonly ChangeContext _changeContext;
    private readonly ImageContext _imageContext;
    private readonly IUserService _userService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IConfiguration _configuration;
    
    public Create(ChangeContext changeContext, 
        ImageContext imageContext,
        IUserService userService, 
        IDateTimeService dateTimeService,
        IConfiguration configuration)
    {
        _changeContext = changeContext;
        _imageContext = imageContext;
        _dateTimeService = dateTimeService;
        _userService = userService;
        _configuration = configuration;
    }

    // Cancellation token not used as that could result the application to be in an invalid state
    [HttpPost("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] CreateImageDto request,
        CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var imageToCreate = new Image(request.Id, request.TakenOn, request.IsFavourite);
        _imageContext
            .Images
            .Add(imageToCreate);
        await _imageContext.SaveChangesAsync();

        var url = _configuration.GetValue<string>("Image:GetImageUrl");
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

        var connectionString = _configuration.GetValue<string>("Image:ConnectionString");
        var client = new BlobClient(connectionString, "images", imageToCreate.TimeSensitiveId);
        await client.UploadAsync(new BinaryData(request.Data));

        return NoContent();
    }
}
