using System.Text.Json;
using Ardalis.ApiEndpoints;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Couple.Api.Features.Issue;

public class Delete : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult
{
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    private readonly IDateTimeService _dateTimeService;
    
    public Delete(ChangeContext context, 
        IUserService userService, 
        IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _userService = userService;
    }

    [HttpDelete("api/[namespace]/{id:guid}")]
    public override async Task<ActionResult> HandleAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var toDelete = new CachedChange(Guid.NewGuid(),
            Command.DeleteIssue,
            claims.PartnerId,
            _dateTimeService.Now,
            id.ToString(),
            JsonSerializer.Serialize(id));
        
        _context
            .CachedChanges
            .Add(toDelete);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
