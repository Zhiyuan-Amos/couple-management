using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model.Change;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Couple.Api.Features.Change;

public class DeleteChangesFunction
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ChangeContext _context;

    public DeleteChangesFunction(ICurrentUserService currentUserService,
        ChangeContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    [Function("DeleteChangeFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Changes")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var form = await req.GetJsonBody<DeleteChangeDto, Validator>();

        if (!form.IsValid)
        {
            var logger = executionContext.GetLogger(GetType().Name);
            var errorMessage = form.ErrorMessage();
            logger.LogWarning("{ErrorMessage}", errorMessage);
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync(errorMessage);
            return response;
        }

        var model = form.Value;

        var toDelete = await _context
            .Changes
            .Where(change => model.Guids.Contains(change.Id))
            .ToListAsync();

        var missingIds = model.Guids
            .Except(toDelete.Select(change => change.Id))
            .ToList();

        if (missingIds.Count != 0)
        {
            var logger = executionContext.GetLogger(GetType().Name);
            logger.LogWarning("Changes of {Ids} are not found", string.Join(", ", missingIds));
        }

        var claims = _currentUserService.GetClaims(req.Headers);
        var canDelete = toDelete.All(change => change.UserId == claims.Id);

        if (!canDelete)
        {
            return req.CreateResponse(HttpStatusCode.Forbidden);
        }

        foreach (var change in toDelete)
        {
            change.Ttl = 3600;
        }

        _context
            .Changes
            .UpdateRange(toDelete);

        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }

    public class Validator : AbstractValidator<DeleteChangeDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.Guids).NotEmpty();
            RuleForEach(dto => dto.Guids).NotEmpty();
        }
    }
}
