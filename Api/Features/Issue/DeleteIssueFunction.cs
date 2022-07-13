using System.Net;
using System.Text.Json;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Features.Issue;

public class DeleteIssueFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public DeleteIssueFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("DeleteIssueFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Issues/{id:guid}")]
        HttpRequestData req,
        Guid id)
    {
        var claims = _currentUserService.GetClaims(req.Headers);

        var toCreate = new CachedChange(Guid.NewGuid(),
            Command.Delete,
            claims.PartnerId,
            _dateTimeService.Now,
            id.ToString(),
            Entity.Issue,
            JsonSerializer.Serialize(id));

        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
