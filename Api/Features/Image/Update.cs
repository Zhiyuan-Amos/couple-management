using Ardalis.ApiEndpoints;
using Azure.Storage.Blobs;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Image;
using Microsoft.AspNetCore.Mvc;

namespace Couple.Api.Features.Image;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateImageDto>
    .WithActionResult
{
    private readonly ChangeContext _changeContext;
    private readonly ImageContext _imageContext;
    private readonly IUserService _userService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IConfiguration _configuration;
    
    public Update(ChangeContext changeContext, 
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
    [HttpPut("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateImageDto request,
        CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var imageToUpdate = new Image(request.Id, request.TakenOn, request.IsFavourite);
        _imageContext
            .Images
            .Add(imageToUpdate);
        await _imageContext.SaveChangesAsync();

        var url = _configuration.GetValue<string>("Image:GetImageUrl");
        var toUpdate = new HyperlinkChange(Guid.NewGuid(),
            Command.Update,
            claims.PartnerId,
            _dateTimeService.Now,
            imageToUpdate.TimeSensitiveId,
            Entity.Image,
            url);

        _changeContext
            .HyperlinkChanges
            .Add(toUpdate);
        await _changeContext.SaveChangesAsync();

        var connectionString = _configuration.GetValue<string>("Image:ConnectionString");
        var client = new BlobClient(connectionString, "images", imageToUpdate.TimeSensitiveId);
        await client.UploadAsync(new BinaryData(request.Data));

        return NoContent();
    }
}
