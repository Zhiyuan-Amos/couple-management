namespace Couple.Messaging.Model;

public class Image
{
    public Guid Id { get; init; } // Unused, but required for integration with CosmosDB
    public Guid ObjectId { get; init; }
    public DateTime TakenOn { get; init; }
    public bool IsFavourite { get; init; }
    public string TimeSensitiveId { get; init; }
}
