using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        [Function("DeleteChangeFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Changes")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var form = await req.GetJsonBody<DeleteChangeDto, Validator>();

            if (!form.IsValid)
            {
                var logger = executionContext.GetLogger(GetType().Name);
                var errorMessage = form.ErrorMessage();
                logger.LogWarning("{ErrorMessage}", errorMessage);
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync(errorMessage);
                return response;
            }

            var model = form.Value;

            var toDelete = await _context
                .Changes
                .Where(change => model.Guids.Contains(change.Id))
                .ToListAsync();

            var areIdsValid = model.Guids.Count == toDelete.Count;

            if (!areIdsValid)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var claims = _currentUserService.GetClaims(req.Headers);
            var canDelete = toDelete.All(change => change.UserId == claims.Id);

            if (!canDelete)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            _context
                .Changes
                .RemoveRange(toDelete);
            await _context.SaveChangesAsync();

            var imageIdsToDelete = toDelete
                .Where(change => change.Command == Command.CreateImage || change.Command == Command.UpdateImage)
                .Select(change => change.Content)
                .Select(content => JsonSerializer.Deserialize<Model.Image>(content))
                .Select(image => image.Id.ToString())
                .ToList();

            var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
            foreach (var id in imageIdsToDelete)
            {
                var client = new BlobClient(connectionString, "images", id);
                await client.DeleteIfExistsAsync();
            }

            return req.CreateResponse(HttpStatusCode.OK);
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
