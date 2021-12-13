using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Api.Validators;
using Couple.Shared.Model;
using Couple.Shared.Model.Event;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Couple.Api.Features.Event;

public class UpdateEventFunction
{
    private readonly ChangeContext _changeContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public UpdateEventFunction(ChangeContext changeContext,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _changeContext = changeContext;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    [Function("UpdateEventFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Events")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var form = await req.GetJsonBody<UpdateEventDto, Validator>();

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
            form.Value.Event.Id.ToString(),
            Entity.Event,
            form.Json);

        _changeContext
            .CachedChanges
            .Add(toCreate);
        await _changeContext.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.OK);
    }

    private class Validator : AbstractValidator<UpdateEventDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.Event).NotNull();
            RuleFor(dto => dto.Event).SetValidator(new EventDtoValidator());
            RuleFor(dto => dto.Added).NotNull();
            RuleForEach(dto => dto.Added).NotEmpty();
            RuleFor(dto => dto.Removed).NotNull();
            RuleForEach(dto => dto.Removed).SetValidator(new IssueDtoValidator());
        }
    }
}
