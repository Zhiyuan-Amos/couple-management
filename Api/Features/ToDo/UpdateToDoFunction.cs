using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.ToDo;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Api.Features.ToDo
{
    public class UpdateToDoFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateToDoFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("UpdateToDoFunction")]
        public async Task<ActionResult> UpdateToDo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ToDos")]
            HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<UpdateToDoDto, Validator>();

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

        private class Validator : AbstractValidator<UpdateToDoDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.Name).NotEmpty();
                RuleFor(dto => dto.For).NotNull();
                RuleFor(dto => dto.ToDos).NotNull();
                RuleForEach(dto => dto.ToDos)
                    .ChildRules(toDos =>
                        toDos.RuleFor(toDo => toDo.Content).NotEmpty());
                RuleFor(dto => dto.CreatedOn).NotEmpty();
            }
        }
    }
}
