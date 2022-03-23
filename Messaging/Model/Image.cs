namespace Couple.Messaging.Model;

public class Image
{
#pragma warning disable IDE0051
    // ReSharper disable once UnusedMember.Local
    private Image(Guid id, Guid objectId, DateTime takenOn, bool isFavourite, string timeSensitiveId)
    {
        Id = id;
        ObjectId = objectId;
        TakenOn = takenOn;
        IsFavourite = isFavourite;
        TimeSensitiveId = timeSensitiveId;
    }
#pragma warning restore IDE0051

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public Guid Id { get; init; } // Unused, but required for integration with CosmosDB
    public Guid ObjectId { get; init; }
    public DateTime TakenOn { get; init; }
    public bool IsFavourite { get; init; }
    public string TimeSensitiveId { get; init; }
}
