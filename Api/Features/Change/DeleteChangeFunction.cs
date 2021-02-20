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
    public class DeleteChangesFunction
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ChangeContext _context;

        public DeleteChangesFunction(ICurrentUserService currentUserService,
                                     ChangeContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        [FunctionName("DeleteChangeFunction")]
        public async Task<ActionResult> DeleteChange(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Changes")] HttpRequest req,
            ILogger log)
        {
            var form = await req.GetJsonBody<DeleteChangeDto, Validator>();

            if (!form.IsValid)
            {
                log.LogWarning(form.ErrorMessage());
                return form.ToBadRequest();
            }

            var model = form.Value;

            var toDelete = await _context
                .Changes
                .Where(change => model.Guids.Contains(change.Id))
                .ToListAsync();

            var areIdsValid = model.Guids.Count == toDelete.Count;

            if (!areIdsValid)
            {
                return new BadRequestResult();
            }

            var canDelete = toDelete.All(change => change.UserId == _currentUserService.Id);

            if (!canDelete)
            {
                return new ForbidResult();
            }

            _context
                .Changes
                .RemoveRange(toDelete);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public class Validator : AbstractValidator<DeleteChangeDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Guids).NotEmpty();
                RuleForEach(dto => dto.Guids).NotEmpty();
            }
        }
    }
}
