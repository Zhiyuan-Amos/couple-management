namespace Couple.Api.Model;

public class Image
{
    public Image(Guid id, DateTime takenOn, bool isFavourite)
    {
        Id = id;
        TakenOn = takenOn;
        IsFavourite = isFavourite;
    }

    public Guid Id { get; }
    public DateTime TakenOn { get; }
    public bool IsFavourite { get; }
}
