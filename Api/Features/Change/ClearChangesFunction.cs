using System.Net;
using Couple.Api.Data;
using Couple.Api.Infrastructure;

namespace Couple.Api.Features.Change;

public class ClearChangesFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ClearChangesFunction(ICurrentUserService currentUserService,
        ChangeContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    [Function("ClearChangesFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Changes/all")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var claims = _currentUserService.GetClaims(req.Headers);
        var toDelete = await _context
            .Changes
            .Where(change => change.UserId == claims.Id || change.UserId == claims.PartnerId)
            .ToListAsync();

        foreach (var change in toDelete)
        {
            change.Ttl = 3600;
        }

        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
