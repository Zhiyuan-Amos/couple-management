using Couple.Shared.Models;

namespace Couple.Api.Shared.Models;

public abstract class Change
{
    protected Change(Guid id,
        Command command,
        string userId,
        DateTime timestamp,
        string contentId)
    {
        Id = id;
        Command = command;
        UserId = userId;
        Timestamp = timestamp;
        ContentId = contentId;

        // 1. Annotating this property with Json Attributes doesn't seem to work e.g.
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] and [JsonPropertyName("ttl")]
        // 2. The alternative is to implement the converter
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to?pivots=dotnet-6-0#conditionally-ignore-a-property
        // which is much more troublesome than this solution.
        Ttl = -1;
    }
    
#pragma warning disable CS8618
    protected Change(Guid id,
#pragma warning restore CS8618
        string userId,
        DateTime timestamp,
        string contentId)
    {
        Id = id;
        UserId = userId;
        Timestamp = timestamp;
        ContentId = contentId;
    }

    public Guid Id { get; init; }
    public Command Command { get; init; }
    public string UserId { get; init; }
    public DateTime Timestamp { get; init; }
    public string ContentId { get; init; }
    public int? Ttl { get; set; }
}
