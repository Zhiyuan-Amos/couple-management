using System.Text.Json.Serialization;

namespace Couple.Messaging.Model;

public class Change
{
    public Guid Id { get; init; }
    public string? Command { get; init; }
    public string? UserId { get; init; }
    public DateTime Timestamp { get; init; }
    public Guid ContentId { get; init; }
    public string ContentType { get; init; }
    public string? Content { get; init; }

    [JsonPropertyName("ttl")] public int? Ttl { get; set; }
}
