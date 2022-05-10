namespace Couple.Api.Model;

public abstract class Change
{
    protected Change(Guid id,
        string command,
        string userId,
        DateTime timestamp,
        string contentId,
        string contentType)
    {
        Id = id;
        Command = command;
        UserId = userId;
        Timestamp = timestamp;
        ContentId = contentId;
        ContentType = contentType;

        // 1. Annotating this property with Json Attributes doesn't seem to work e.g.
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] and [JsonPropertyName("ttl")]
        // 2. The alternative is to implement the converter
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to?pivots=dotnet-6-0#conditionally-ignore-a-property
        // which is much more troublesome than this solution.
        Ttl = -1;
    }

    public Guid Id { get; init; }
    public string Command { get; init; }
    public string UserId { get; init; }
    public DateTime Timestamp { get; init; }
    public string ContentId { get; init; }
    public string ContentType { get; init; }
    public int? Ttl { get; set; }
}
