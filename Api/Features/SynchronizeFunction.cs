using Api.Data;
using Api.Infrastructure;
using Couple.Shared.Model;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features
{
    public class SynchronizeFunction
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly EventContext _context;

        public SynchronizeFunction(ICurrentUserService currentUserService,
                                   EventContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        [FunctionName("SynchronizeFunction")]
        public async Task<ActionResult> Synchronize(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "synchronize")] HttpRequest req,
            ILogger log)
        {
            var toReturn = (await _context
                .Changes
                .Where(change => change.UserId == _currentUserService.Id)
                .OrderBy(change => change.Timestamp)
                .ToListAsync())
                .Select(change => new ChangeDto
                {
                    Id = change.Id,
                    Function = change.Function,
                    DataType = change.DataType,
                    UserId = change.UserId,
                    Timestamp = change.Timestamp,
                    Content = change.Content,
                });

            return new OkObjectResult(toReturn);
        }
    }
}
