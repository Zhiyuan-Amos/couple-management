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
    public class UpdateIssueFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateIssueFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [Function("UpdateIssueFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Issues")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var form = await req.GetJsonBody<UpdateIssueDto, Validator>();

            if (!form.IsValid)
            {
                var logger = executionContext.GetLogger(GetType().Name);
                var errorMessage = form.ErrorMessage();
                logger.LogWarning("{ErrorMessage}", errorMessage);
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync(errorMessage);
                return response;
            }

            var claims = _currentUserService.GetClaims(req.Headers);
            if (claims.PartnerId == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var toCreate = new Model.Change(Guid.NewGuid(),
                Command.UpdateIssue,
                claims.PartnerId,
                _dateTimeService.Now,
                form.Json);

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return req.CreateResponse(HttpStatusCode.OK);
        }

        private class Validator : AbstractValidator<UpdateIssueDto>
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
