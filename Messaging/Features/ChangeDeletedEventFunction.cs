using System.Text.Json;
using Couple.Messaging.Model;
using Couple.Shared.Model;

namespace Couple.Messaging.Features;

public class ChangeDeletedEventFunction
{
    [FunctionName("ChangeDeletedEventFunction")]
    public async Task Run([CosmosDBTrigger("%DatabaseName%",
            "%CollectionName%",
            ConnectionStringSetting = "DatabaseConnectionString",
            CreateLeaseCollectionIfNotExists = true)]
        IReadOnlyList<Document> documents,
        [EventGrid(TopicEndpointUri = "EventGridEndpoint",
            TopicKeySetting = "EventGridKey")]
        IAsyncCollector<EventGridEvent> eventCollector)
    {
        var changes = documents.Select(d => d.ToString())
            .Select(json => JsonSerializer.Deserialize<Change>(json)!)
            .Where(change => change.Ttl != -1)
            .Where(change => change.ContentType == Entity.Image && change.Command is Command.Create or Command.Update)
            .ToList();

        List<Task> tasks = new();
        foreach (var change in changes)
        {
            var task = eventCollector.AddAsync(new(change.ContentId, "ImageDeleted", "1", new { }));
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}
