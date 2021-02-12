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
    public class CreateToDoFunction
    {
        private readonly EventContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CreateToDoFunction(EventContext context,
                                  IDateTimeService dateTimeService,
                                  ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("CreateToDoFunction")]
        public async Task<ActionResult> CreateToDo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ToDos")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<CreateToDoDto, Validator>();

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

        public class Validator : AbstractValidator<CreateToDoDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.Text).NotEmpty();
                RuleFor(dto => dto.Category).NotEmpty();
                RuleFor(dto => dto.CreatedOn).NotEmpty();
            }
        }
    }
}
