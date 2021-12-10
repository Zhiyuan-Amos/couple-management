using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Features.Issue;

public class DeleteIssueFunction
{
    private readonly ChangeContext _context;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;

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
        if (claims.PartnerId == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var toCreate = new Model.CachedChange(Guid.NewGuid(),
            Command.Delete,
            claims.PartnerId,
            _dateTimeService.Now,
            id,
            Entity.Issue,
            JsonSerializer.Serialize(id));

        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
