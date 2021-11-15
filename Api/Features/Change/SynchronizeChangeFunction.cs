using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Storage.Blobs;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Image;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;

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

        [Function("SynchronizeChangeFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Synchronize")]
            HttpRequestData req)
        {
            // Running out of memory (1.5GB, see https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/azure-subscription-service-limits#azure-functions-limits)
            // or sending massive amounts of data to the Client in an instance is not expected to occur
            // as Synchronization is expected to happen frequently, and the synchronized data is deleted from the
            // database.
            var claims = _currentUserService.GetClaims(req.Headers);
            var toReturn = await _context
                .Changes
                .Where(change => change.UserId == claims.Id)
                .OrderBy(change => change.Timestamp)
                .ProjectTo<ChangeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var change in toReturn)
            {
                if (change.Command != Command.CreateImage && change.Command != Command.UpdateImage)
                {
                    continue;
                }

                var image = JsonSerializer.Deserialize<Model.Image>(change.Content);

                var connectionString = Environment.GetEnvironmentVariable("ImagesConnectionString");
                var client = new BlobClient(connectionString, "images", image!.Id.ToString());
                var stream = new MemoryStream();
                await client.DownloadToAsync(stream);
                var data = stream.ToArray();
                await stream.ReadAsync(data.AsMemory(0, (int)stream.Length));

                if (change.Command == Command.CreateImage)
                {
                    var toSerialize = new CreateImageDto
                    {
                        Id = image.Id,
                        TakenOn = image.TakenOn,
                        Data = data,
                        IsFavourite = image.IsFavourite,
                    };
                    change.Content = JsonSerializer.Serialize(toSerialize);
                }
                else if (change.Command == Command.UpdateImage)
                {
                    var toSerialize = new UpdateImageDto
                    {
                        Id = image.Id,
                        TakenOn = image.TakenOn,
                        Data = data,
                        IsFavourite = image.IsFavourite,
                    };
                    change.Content = JsonSerializer.Serialize(toSerialize);
                }
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(toReturn);
            return response;
        }
    }
}
