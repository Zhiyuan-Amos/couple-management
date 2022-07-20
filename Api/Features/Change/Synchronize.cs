using System.Text.Json;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Couple.Api.Shared.Models;
using Couple.Shared.Models;
using Couple.Shared.Models.Change;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Features.Change;

public class Synchronize : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly HttpClient _client;
    private readonly ChangeContext _context;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<Synchronize> _logger;
    
    public Synchronize(IHttpClientFactory httpClientFactory,
        ChangeContext context, 
        IUserService userService,
        IMapper mapper,
        ILogger<Synchronize> logger)
    {
        _client = httpClientFactory.CreateClient("Image");
        _context = context;
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        // Sending massive amounts of data to the Client in an instance 
        // is not expected to occur as Synchronization is expected to happen frequently, 
        // and the synchronized data is deleted from the database.
        // Therefore, it should be unlikely for the Server to run into OOM.
        var claims = _userService.GetClaims(Request.Headers);
        var changes = await _context
            .Changes
            .Where(change => change.UserId == claims.Id)
            .Where(change => change.Ttl == -1)
            .OrderBy(change => change.Timestamp)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var hyperlinkChanges = changes.OfType<HyperlinkChange>().ToList();
        Dictionary<string, HyperlinkContent> imageIdToImage = new();
        if (hyperlinkChanges.Count > 0)
        {
            var url = hyperlinkChanges[0].Url;
            var ids = hyperlinkChanges
                .Select(hyperlinkChange => hyperlinkChange.ContentId)
                .ToList();
            // See https://www.elastic.co/guide/en/elasticsearch/guide/current/_empty_search.html
            var httpResponseMessage = await _client.PostAsJsonAsync(url, ids, cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Request to retrieve images is unsuccessful");
            }

            var images = (await httpResponseMessage.Content.ReadFromJsonAsync<List<HyperlinkContent>>(cancellationToken: cancellationToken))!;
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
                    _logger.LogWarning("Image of {Id} is not found", change.ContentId);
                    continue;
                }

                var toAdd = new ChangeDto(change.Id, change.Command, change.ContentType,
                    JsonSerializer.Serialize(image.Content));
                toReturn.Add(toAdd);
            }
        }

        return Ok(toReturn);
    }
}
