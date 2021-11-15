using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Features.Image
{
    public class DeleteImageFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteImageFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [Function("DeleteImageFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Images/{id:guid}")]
            HttpRequestData req,
            Guid id)
        {
            var claims = _currentUserService.GetClaims(req.Headers);
            if (claims.PartnerId == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var toCreate = new Model.Change(Guid.NewGuid(),
                Command.DeleteImage,
                claims.PartnerId,
                _dateTimeService.Now,
                JsonSerializer.Serialize(id));

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
