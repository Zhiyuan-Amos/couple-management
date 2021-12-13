using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace Couple.Api.Features.Event;

public class DeleteChangesFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public DeleteChangesFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("DeleteEventFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Events/{id:guid}")]
        HttpRequestData req,
        Guid id)
    {
        var claims = _currentUserService.GetClaims(req.Headers);
        if (claims.PartnerId == null) return req.CreateResponse(HttpStatusCode.BadRequest);

        var toCreate = new CachedChange(Guid.NewGuid(),
            Command.Delete,
            claims.PartnerId,
            _dateTimeService.Now,
            id.ToString(),
            Entity.Event,
            JsonSerializer.Serialize(id));

        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
