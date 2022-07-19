using Ardalis.ApiEndpoints;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Shared.Models.Change;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Features.Change;

public class Delete : EndpointBaseAsync
    .WithRequest<DeleteChangesDto>
    .WithActionResult
{
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    private readonly ILogger<Delete> _logger;
    
    public Delete(ChangeContext context, 
        IUserService userService,
        ILogger<Delete> logger)
    {
        _context = context;
        _userService = userService;
        _logger = logger;
    }

    [HttpDelete("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] DeleteChangesDto request,
        CancellationToken cancellationToken)
    {
        var toDelete = await _context
            .Changes
            .Where(change => request.Guids.Contains(change.Id))
            .ToListAsync(cancellationToken);

        var claims = _userService.GetClaims(Request.Headers);
        var canDelete = toDelete.All(change => change.UserId == claims.Id);

        if (!canDelete)
        {
            return Forbid();
        }

        var missingIds = request.Guids
            .Except(toDelete.Select(change => change.Id))
            .ToList();

        if (missingIds.Count != 0)
        {
            _logger.LogWarning("Changes of {Ids} are not found", string.Join(", ", missingIds));
        }

        foreach (var change in toDelete)
        {
            change.Ttl = 3600;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
