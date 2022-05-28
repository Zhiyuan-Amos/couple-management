namespace Couple.Messaging.Features.Image;

public class ImageDto
{
    public ImageDto(Guid id, DateTime takenOn, byte[] data, bool isFavourite)
    {
        Id = id;
        TakenOn = takenOn;
        Data = data;
        IsFavourite = isFavourite;
    }

    public Guid Id { get; }
    public DateTime TakenOn { get; }
    public byte[] Data { get; }
    public bool IsFavourite { get; }
}
