using System.Text.Json;
using Ardalis.ApiEndpoints;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Issue;
using Microsoft.AspNetCore.Mvc;

namespace Couple.Api.Features.Issue.Task;

public class Complete : EndpointBaseAsync
    .WithRequest<CompleteTaskDto>
    .WithActionResult
{
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    private readonly IDateTimeService _dateTimeService;
    
    public Complete(ChangeContext context, 
        IUserService userService, 
        IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _userService = userService;
    }

    [HttpPut("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] CompleteTaskDto request,
        CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var toComplete = new CachedChange(Guid.NewGuid(),
            Command.Complete,
            claims.PartnerId,
            _dateTimeService.Now,
            request.TaskId.ToString(),
            Entity.Task,
            JsonSerializer.Serialize(request));
        
        _context
            .CachedChanges
            .Add(toComplete);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
