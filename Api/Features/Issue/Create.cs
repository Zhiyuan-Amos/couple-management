using System.Text.Json;
using Ardalis.ApiEndpoints;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Issue;
using Microsoft.AspNetCore.Mvc;

namespace Couple.Api.Features.Issue;

public class Create : EndpointBaseAsync
    .WithRequest<CreateIssueDto>
    .WithActionResult
{
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    private readonly IDateTimeService _dateTimeService;
    
    public Create(ChangeContext context, 
        IUserService userService, 
        IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _userService = userService;
    }

    [HttpPost("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] CreateIssueDto request,
        CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var toCreate = new CachedChange(Guid.NewGuid(),
            Command.CreateIssue,
            claims.PartnerId,
            _dateTimeService.Now,
            request.Id.ToString(),
            JsonSerializer.Serialize(request));
        
        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
