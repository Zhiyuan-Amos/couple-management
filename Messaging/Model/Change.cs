namespace Couple.Messaging.Model;

public class Change
{
    [JsonConstructor]
    public Change(Guid id,
        string command,
        string userId,
        DateTime timestamp,
        string contentId,
        string contentType,
        int? ttl)
    {
        (Id, Command, UserId, Timestamp, ContentId, ContentType, Ttl) =
            (id, command, userId, timestamp, contentId, contentType, ttl);
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Guid Id { get; }
    public string Command { get; }
    public string UserId { get; }
    public DateTime Timestamp { get; }
    public string ContentId { get; }
    public string ContentType { get; }

    [JsonPropertyName("ttl")] public int? Ttl { get; set; }
}