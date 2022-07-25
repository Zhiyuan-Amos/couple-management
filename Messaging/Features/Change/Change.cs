using System.Text.Json.Serialization;
using Couple.Shared.Models;

namespace Couple.Messaging.Features.Change;

public class Change
{
    [JsonConstructor]
    public Change(Guid id,
        Command command,
        string userId,
        DateTime timestamp,
        string contentId,
        int? ttl) => (Id, Command, UserId, Timestamp, ContentId, Ttl) =
        (id, command, userId, timestamp, contentId, ttl);

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Guid Id { get; }
    public Command Command { get; }
    public string UserId { get; }
    public DateTime Timestamp { get; }
    public string ContentId { get; }

    [JsonPropertyName("ttl")] public int? Ttl { get; set; }
}
