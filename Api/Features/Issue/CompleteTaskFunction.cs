using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Issue;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Couple.Api.Features.Issue
{
    public class CompleteTaskFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CompleteTaskFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("CompleteTaskFunction")]
        public async Task<ActionResult> CompleteTask(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Tasks/Complete")]
            HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<CompleteTaskDto, Validator>();

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
                Command = Command.CompleteTask,
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

        private class Validator : AbstractValidator<CompleteTaskDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.For).NotNull();
                RuleFor(dto => dto.Content).NotEmpty();
                RuleFor(dto => dto.IssueId).NotEmpty();
                RuleFor(dto => dto.IssueTitle).NotEmpty();
                RuleFor(dto => dto.CreatedOn).NotEmpty();
            }
        }
    }
}
