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
    public class CreateEventFunction
    {
        private readonly EventContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CreateEventFunction(EventContext context,
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
                Function = Function.Create,
                DataType = DataType.Calendar,
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

        public class Validator : AbstractValidator<CreateEventDto>
        {
            public Validator()
            {
            }
        }
    }
}
