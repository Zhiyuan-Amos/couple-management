namespace Couple.Messaging.Model;

public class Image
{
    public Image(Guid id, DateTime takenOn, bool isFavourite)
    {
        Id = id;
        TakenOn = takenOn;
        IsFavourite = isFavourite;
        TimeSensitiveId = $"{Id}_{TakenOn.Ticks / TimeSpan.TicksPerMillisecond}";
    }

    public Guid Id { get; init; }
    public DateTime TakenOn { get; init; }
    public bool IsFavourite { get; init; }
    public string TimeSensitiveId { get; init; }
}
