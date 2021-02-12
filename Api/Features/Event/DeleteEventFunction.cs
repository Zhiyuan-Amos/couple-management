using Api.Data;
using Api.Infrastructure;
using Couple.Api.Validators;
using Couple.Shared.Model;
using Couple.Shared.Model.Calendar;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Features.Event
{
    public class DeleteChangesFunction
    {
        private readonly EventContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteChangesFunction(EventContext context,
                                     IDateTimeService dateTimeService,
                                     ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("DeleteEventFunction")]
        public async Task<ActionResult> DeleteEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Events")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<DeleteEventDto, Validator>();

            if (!form.IsValid)
            {
                log.LogWarning(form.ErrorMessage());
                return form.ToBadRequest();
            }

            var model = form.Value;

            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var toCreate = new Change
            {
                Id = Guid.NewGuid(),
                Function = Function.Delete,
                DataType = DataType.Calendar,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = JsonSerializer.Serialize(model),
            };

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public class Validator : AbstractValidator<DeleteEventDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.Removed).NotNull();
                RuleForEach(dto => dto.Removed).SetValidator(new ToDoDtoValidator());
            }
        }
    }
}
