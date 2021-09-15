using Azure.Storage.Blobs;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
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
                log.LogWarning("{ErrorMessage}", form.ErrorMessage());
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

            var imageIdsToDelete = toDelete
                .Where(change => change.Command == Command.CreateImage)
                .Select(change => change.Content)
                .Select(content => JsonSerializer.Deserialize<Model.Image>(content))
                .Select(image => image.Id.ToString())
                .ToList();

            var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString")!;
            foreach (var id in imageIdsToDelete)
            {
                var client = new BlobClient(connectionString, "images", id);
                await client.DeleteIfExistsAsync();
            }

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
