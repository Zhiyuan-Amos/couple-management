using Ardalis.ApiEndpoints;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Features.Change;

public class Clear : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    
    public Clear(ChangeContext context, 
        IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpDelete("api/[namespace]s")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var toDelete = await _context
            .Changes
            .Where(change => change.UserId == claims.Id || change.UserId == claims.PartnerId)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var change in toDelete)
        {
            change.Ttl = 3600;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
