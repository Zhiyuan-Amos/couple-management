using Couple.Api.Data;
using Couple.Api.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Couple.Shared.Model.Change;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Api.Features.Change
{
    public class SynchronizeChangeFunction
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ChangeContext _context;

        public SynchronizeChangeFunction(ICurrentUserService currentUserService,
                                   ChangeContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        [FunctionName("SynchronizeChangeFunction")]
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
