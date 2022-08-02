using System.Text.Json;
using Ardalis.ApiEndpoints;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace Couple.Api.Features.Event;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateEventDto>
    .WithActionResult
{
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    private readonly IDateTimeService _dateTimeService;
    
    public Update(ChangeContext context, 
        IUserService userService, 
        IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _userService = userService;
    }

    [HttpPut("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateEventDto request,
        CancellationToken cancellationToken)
    {
        var claims = _userService.GetClaims(Request.Headers);

        var toUpdate = new CachedChange(Guid.NewGuid(),
            Command.UpdateEvent,
            claims.PartnerId,
            _dateTimeService.Now,
            request.Id.ToString(),
            JsonSerializer.Serialize(request));
        
        _context
            .CachedChanges
            .Add(toUpdate);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
