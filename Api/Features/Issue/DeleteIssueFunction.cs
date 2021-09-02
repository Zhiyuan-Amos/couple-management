using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Couple.Api.Features.Issue
{
    public class DeleteIssueFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteIssueFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("DeleteIssueFunction")]
        public async Task<ActionResult> DeleteIssue(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Issues/{id:guid}")] HttpRequest req,
            Guid id,
            ILogger log)
        {
            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var toCreate = new Model.Change
            {
                Id = Guid.NewGuid(),
                Command = Command.DeleteIssue,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = JsonSerializer.Serialize(id),
            };

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}