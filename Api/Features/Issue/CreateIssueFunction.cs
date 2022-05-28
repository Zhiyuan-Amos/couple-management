using System.Net;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Extensions;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Issue;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Couple.Api.Features.Issue;

public class CreateIssueFunction
{
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public CreateIssueFunction(ChangeContext context,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("CreateIssueFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Issues")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var form = await req.GetJsonBody<CreateIssueDto, Validator>();

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

        var toCreate = new CachedChange(Guid.NewGuid(),
            Command.Create,
            claims.PartnerId,
            _dateTimeService.Now,
            form.Value.Id.ToString(),
            Entity.Issue,
            form.Json);

        _context
            .CachedChanges
            .Add(toCreate);
        await _context.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private class Validator : AbstractValidator<CreateIssueDto>
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
