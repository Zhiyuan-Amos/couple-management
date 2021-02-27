using AutoMapper;
using AutoMapper.QueryableExtensions;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model.Change;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Api.Features.Change
{
    public class SynchronizeChangeFunction
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ChangeContext _context;

        public SynchronizeChangeFunction(ICurrentUserService currentUserService,
                                         IMapper mapper,
                                         ChangeContext context)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context = context;
        }

        [FunctionName("SynchronizeChangeFunction")]
        public async Task<ActionResult> Synchronize(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "synchronize")] HttpRequest req,
            ILogger log)
        {
            var toReturn = await _context
                .Changes
                .Where(change => change.UserId == _currentUserService.Id)
                .OrderBy(change => change.Timestamp)
                .ProjectTo<ChangeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new OkObjectResult(toReturn);
        }
    }
}
