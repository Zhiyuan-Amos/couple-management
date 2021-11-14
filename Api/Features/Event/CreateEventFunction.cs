using System;
using System.Threading.Tasks;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Validators;
using Couple.Shared.Model;
using Couple.Shared.Model.Event;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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

        [FunctionName("CreateEventFunction")]
        public async Task<ActionResult> CreateEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Events")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<CreateEventDto, Validator>();

            if (!form.IsValid)
            {
                log.LogWarning("{ErrorMessage}", form.ErrorMessage());
                return form.ToBadRequest();
            }

            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var toCreate = new Model.Change
            {
                Id = Guid.NewGuid(),
                Command = Command.CreateEvent,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = form.Json,
            };

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return new OkResult();
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
