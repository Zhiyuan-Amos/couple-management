using Api.Data;
using Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Calendar;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Features.Event
{
    public class UpdateEventFunction
    {
        private readonly EventContext _eventContext;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEventFunction(EventContext eventContext,
                                   IDateTimeService dateTimeService,
                                   ICurrentUserService currentUserService)
        {
            _eventContext = eventContext;
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
                log.LogWarning(form.ErrorMessage());
                return form.ToBadRequest();
            }

            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var toCreate = new Change
            {
                Id = Guid.NewGuid(),
                Function = Function.Update,
                DataType = DataType.Calendar,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = form.Json,
            };

            _eventContext
                .Changes
                .Add(toCreate);
            await _eventContext.SaveChangesAsync();

            return new OkResult();
        }

        public class Validator : AbstractValidator<UpdateEventDto>
        {
            public Validator()
            {
            }
        }
    }
}
