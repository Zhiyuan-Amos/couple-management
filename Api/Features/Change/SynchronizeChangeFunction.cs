using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Couple.Api.Features.Change;

public class SynchronizeChangeFunction
{
    private readonly HttpClient _client;
    private readonly ChangeContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public SynchronizeChangeFunction(ICurrentUserService currentUserService,
        IHttpClientFactory httpClientFactory,
        IMapper mapper,
        ChangeContext context)
    {
        _currentUserService = currentUserService;
        _client = httpClientFactory.CreateClient("Image");
        _mapper = mapper;
        _context = context;
    }

    [Function("SynchronizeChangeFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Synchronize")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        // Running out of memory (1.5GB, see https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/azure-subscription-service-limits#azure-functions-limits)
        // or sending massive amounts of data to the Client in an instance is not expected to occur
        // as Synchronization is expected to happen frequently, and the synchronized data is deleted from the
        // database.
        var claims = _currentUserService.GetClaims(req.Headers);
        var changes = await _context
            .Changes
            .Where(change => change.UserId == claims.Id)
            .Where(change => change.Ttl == -1)
            .OrderBy(change => change.Timestamp)
            .ToListAsync();

        var hyperlinkChanges = changes.OfType<HyperlinkChange>().ToList();
        Dictionary<string, HyperlinkContent> imageIdToImage = new();
        if (hyperlinkChanges.Count > 0)
        {
            var url = hyperlinkChanges[0].Url;
            var ids = hyperlinkChanges
                .Select(hyperlinkChange => hyperlinkChange.ContentId)
                .ToList();
            // See https://www.elastic.co/guide/en/elasticsearch/guide/current/_empty_search.html
            var result = await _client.PostAsJsonAsync(url, ids);

            var images = (await result.Content.ReadFromJsonAsync<List<HyperlinkContent>>())!;
            imageIdToImage = images.ToDictionary(image => image.ContentId, image => image);
        }

        List<ChangeDto> toReturn = new();
        foreach (var change in changes)
        {
            if (change is CachedChange)
            {
                var toAdd = _mapper.Map<ChangeDto>(change);
                toReturn.Add(toAdd);
            }
            else
            {
                if (!imageIdToImage.TryGetValue(change.ContentId, out var image))
                {
                    var logger = executionContext.GetLogger(GetType().Name);
                    logger.LogWarning("Image of {Id} is not found", change.ContentId);
                    continue;
                }

                var toAdd = new ChangeDto(change.Id, change.Command, change.ContentType,
                    JsonSerializer.Serialize(image.Content));
                toReturn.Add(toAdd);
            }
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(toReturn);
        return response;
    }
}
