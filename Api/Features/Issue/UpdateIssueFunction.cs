using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared.Model;
using Couple.Shared.Model.Issue;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Couple.Api.Features.Issue;

public class UpdateIssueFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public UpdateIssueFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("UpdateIssueFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Issues")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var form = await req.GetJsonBody<UpdateIssueDto, Validator>();

        if (!form.IsValid)
        {
            var logger = executionContext.GetLogger(GetType().Name);
            var errorMessage = form.ErrorMessage();
            logger.LogWarning("{ErrorMessage}", errorMessage);
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync(errorMessage);
            return response;
        }

        var claims = _currentUserService.GetClaims(req.Headers);
        if (claims.PartnerId == null) return req.CreateResponse(HttpStatusCode.BadRequest);

        var toCreate = new CachedChange(Guid.NewGuid(),
            Command.Update,
            claims.PartnerId,
            _dateTimeService.Now,
            form.Value.Id,
            Entity.Issue,
            form.Json);

        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private class Validator : AbstractValidator<UpdateIssueDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.For).NotNull();
            RuleFor(dto => dto.Tasks).NotNull();
            RuleForEach(dto => dto.Tasks)
                .ChildRules(tasks =>
                    tasks.RuleFor(task => task.Content).NotEmpty());
            RuleFor(dto => dto.CreatedOn).NotEmpty();
        }
    }
}
