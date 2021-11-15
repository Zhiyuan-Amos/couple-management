using System;
using System.Net;
using System.Threading.Tasks;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Validators;
using Couple.Shared.Model;
using Couple.Shared.Model.Event;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Couple.Api.Features.Event
{
    public class CreateEventFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CreateEventFunction(ChangeContext context,
                                   IDateTimeService dateTimeService,
                                   ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [Function("CreateEventFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Events")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var form = await req.GetJsonBody<CreateEventDto, Validator>();

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
            if (claims.PartnerId == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var toCreate = new Model.Change(Guid.NewGuid(),
                Command.CreateEvent,
                claims.PartnerId,
                _dateTimeService.Now,
                form.Json);

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return req.CreateResponse(HttpStatusCode.OK);
        }

        private class Validator : AbstractValidator<CreateEventDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Event).NotNull();
                RuleFor(dto => dto.Event).SetValidator(new EventDtoValidator());
                RuleFor(dto => dto.Added).NotNull();
                RuleForEach(dto => dto.Added).NotEmpty();
            }
        }
    }
}
