using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            ILogger log,
            IBinder binder)
        {
            // Running out of memory (1.5GB, see https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/azure-subscription-service-limits#azure-functions-limits)
            // or sending massive amounts of data to the Client in an instance is not expected to occur
            // as Synchronization is expected to happen frequently, and the synchronized data is deleted from the
            // database.
            var toReturn = await _context
                .Changes
                .Where(change => change.UserId == _currentUserService.Id)
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

                var blobAttribute = new BlobAttribute($"images/{image.Id}", FileAccess.Read)
                {
                    Connection = "ImagesConnectionString"
                };
                await using var stream = binder.Bind<Stream>(blobAttribute);
                var data = new byte[stream.Length];
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

            return new OkObjectResult(toReturn);
        }
    }
}
