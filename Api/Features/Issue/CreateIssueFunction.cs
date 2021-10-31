using System;
using System.Threading.Tasks;
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

namespace Couple.Api.Features.Issue
{
    public class CreateIssueFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CreateIssueFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("CreateIssueFunction")]
        public async Task<ActionResult> CreateIssue(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Issues")]
            HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<CreateIssueDto, Validator>();

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
                Command = Command.CreateIssue,
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

        private class Validator : AbstractValidator<CreateIssueDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.Title).NotEmpty();
                RuleFor(dto => dto.For).NotNull();
                RuleFor(dto => dto.Tasks).NotNull();
                RuleForEach(dto => dto.Tasks)
                    .ChildRules(tasks =>
                        tasks.RuleFor(task => task.Content).NotEmpty());
                RuleFor(dto => dto.CreatedOn).NotEmpty();
            }
        }
    }
}
