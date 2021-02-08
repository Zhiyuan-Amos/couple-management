using Api.Data;
using Api.Infrastructure;
using Couple.Api.Data;
using Couple.Shared.Model.User;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Couple.Api.Features.User
{
    public class PairPartnerFunction
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserContext _context;

        public PairPartnerFunction(ICurrentUserService currentUserService,
                                   UserContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        [FunctionName("PairPartnerFunction")]
        public async Task<ActionResult> PairPartner(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Pair")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<PairDto, Validator>();

            if (!form.IsValid)
            {
                log.LogWarning(form.ErrorMessage());
                return form.ToBadRequest();
            }

            var model = form.Value;

            var hasPerformedPairing = await _context
                .Pairs
                .CountAsync(pair => pair.UserIdOne == _currentUserService.Id
                    || pair.UserIdTwo == _currentUserService.Id); // CosmosDb does not support AnyAsync. See https://docs.microsoft.com/en-us/azure/cosmos-db/sql-query-linq-to-sql#SupportedLinqOperators

            if (hasPerformedPairing > 0)
            {
                return new BadRequestResult();
            }

            var existingPair = await _context
                .Pairs
                .SingleOrDefaultAsync(pair => pair.EmailTwo == _currentUserService.Email);

            if (existingPair == null)
            {
                _context
                    .Pairs
                    .Add(new Pair
                    {
                        UserIdOne = _currentUserService.Id,
                        EmailOne = _currentUserService.Email,
                        EmailTwo = model.EmailAddress,
                    });
            }
            else
            {
                if (existingPair.EmailOne != model.EmailAddress)
                {
                    return new BadRequestResult();
                }

                existingPair.UserIdTwo = _currentUserService.Id;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public class Validator : AbstractValidator<PairDto> { }
    }
}
