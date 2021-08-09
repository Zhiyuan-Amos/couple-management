using Couple.Api.Data;
using Couple.Api.Infrastructure;
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

namespace Couple.Api.Features.ToDo
{
    public class CompleteToDoFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CompleteToDoFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("CompleteToDoFunction")]
        public async Task<ActionResult> CompleteToDo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ToDos/Complete")]
            HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<CompleteToDoDto, Validator>();

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
                Command = Command.CompleteToDo,
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

        private class Validator : AbstractValidator<CompleteToDoDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.Name).NotNull();
                RuleFor(dto => dto.For).NotNull();
                RuleFor(dto => dto.ToDos).NotNull();
                RuleForEach(dto => dto.ToDos)
                    .ChildRules(toDos =>
                        toDos.RuleFor(toDo => toDo.Content).NotEmpty());
                RuleFor(dto => dto.CreatedOn).NotEmpty();
                RuleFor(dto => dto.CompletedOn).NotEmpty();
            }
        }
    }
}
