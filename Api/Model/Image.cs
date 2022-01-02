namespace Couple.Api.Model;

public class Image
{
    public Image(Guid objectId, DateTime takenOn, bool isFavourite)
    {
        ObjectId = objectId;
        TakenOn = takenOn;
        IsFavourite = isFavourite;

        Id = Guid.NewGuid();
        TimeSensitiveId = $"{ObjectId}_{TakenOn.Ticks / TimeSpan.TicksPerMillisecond}";
    }

    public Guid Id { get; init; }
    public Guid ObjectId { get; init; }
    public DateTime TakenOn { get; init; }
    public bool IsFavourite { get; init; }
    public string TimeSensitiveId { get; init; }
}
