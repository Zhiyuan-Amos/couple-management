using System.Net;
using System.Text.Json;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Features.Image;

public class DeleteImageFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public DeleteImageFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("DeleteImageFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Images/{id:guid}")]
        HttpRequestData req,
        Guid id)
    {
        var claims = _currentUserService.GetClaims(req.Headers);

        var toCreate = new CachedChange(Guid.NewGuid(),
            Command.Delete,
            claims.PartnerId,
            _dateTimeService.Now,
            id.ToString(),
            Entity.Image,
            JsonSerializer.Serialize(id));

        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
