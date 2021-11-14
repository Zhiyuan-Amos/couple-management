using System;
using System.Net;
using System.Threading.Tasks;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Issue;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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

        [Function("CompleteTaskFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Tasks/Complete")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var form = await req.GetJsonBody<CompleteTaskDto, Validator>();

            if (!form.IsValid)
            {
                var logger = executionContext.GetLogger(GetType().Name);
                var errorMessage = form.ErrorMessage();
                logger.LogWarning("{ErrorMessage}", errorMessage);
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync(errorMessage);
                return response;
            }

            if (_currentUserService.PartnerId == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
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

            return req.CreateResponse(HttpStatusCode.OK);
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
