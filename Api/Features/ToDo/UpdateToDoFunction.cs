using Api.Data;
using Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.ToDo;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Features.ToDo
{
    public class UpdateToDoFunction
    {
        private readonly EventContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateToDoFunction(EventContext context,
                                  IDateTimeService dateTimeService,
                                  ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("UpdateToDoFunction")]
        public async Task<ActionResult> UpdateToDo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ToDos")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<UpdateToDoDto, Validator>();

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
                DataType = DataType.ToDo,
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

        public class Validator : AbstractValidator<UpdateToDoDto>
        {
            public Validator()
            {
                RuleFor(c => c.Id).NotNull();
                RuleFor(c => c.Text).NotEmpty();
            }
        }
    }
}
