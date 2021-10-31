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
    public class UpdateEventFunction
    {
        private readonly ChangeContext _changeContext;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEventFunction(ChangeContext changeContext,
                                   IDateTimeService dateTimeService,
                                   ICurrentUserService currentUserService)
        {
            _changeContext = changeContext;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("UpdateEventFunction")]
        public async Task<ActionResult> UpdateEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Events")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<UpdateEventDto, Validator>();

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
                Command = Command.UpdateEvent,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = form.Json,
            };

            _changeContext
                .Changes
                .Add(toCreate);
            await _changeContext.SaveChangesAsync();

            return new OkResult();
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
}
