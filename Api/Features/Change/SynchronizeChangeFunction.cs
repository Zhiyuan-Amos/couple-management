using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Image;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Couple.Api.Features.Change;

public class SynchronizeChangeFunction
{
    private readonly ICurrentUserService _currentUserService;
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    private readonly ChangeContext _context;

    public SynchronizeChangeFunction(ICurrentUserService currentUserService,
        IHttpClientFactory httpClientFactory,
        IMapper mapper,
        ChangeContext context)
    {
        _currentUserService = currentUserService;
        _client = httpClientFactory.CreateClient();
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
        Dictionary<Guid, byte[]> imageIdToData = new();
        if (hyperlinkChanges.Count > 0)
        {
            var url = hyperlinkChanges[0].Url;
            var ids = hyperlinkChanges
                .Select(hyperlinkChange => hyperlinkChange.ContentId)
                .ToList();
            // See https://www.elastic.co/guide/en/elasticsearch/guide/current/_empty_search.html
            var result = await _client.PostAsJsonAsync(url, ids);

            var images = (await result.Content.ReadFromJsonAsync<List<ImageDto>>())!;
            imageIdToData = images.ToDictionary(image => image.Id, image => image.Data);
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
                if (!imageIdToData.TryGetValue(change.ContentId, out var data))
                {
                    var logger = executionContext.GetLogger(GetType().Name);
                    logger.LogWarning("Image of {Id} is not found", change.ContentId);
                    continue;
                }

                dynamic image = JsonSerializer.Deserialize<ExpandoObject>(change.Content)!;
                image.Data = data;
                var toAdd = new ChangeDto(change.Id, change.Command, change.ContentType,
                    JsonSerializer.Serialize(image));
                toReturn.Add(toAdd);
            }
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(toReturn);
        return response;
    }
}
