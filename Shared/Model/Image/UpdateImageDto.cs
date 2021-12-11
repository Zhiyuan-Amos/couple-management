namespace Couple.Shared.Model.Image;

public class UpdateImageDto
{
    public UpdateImageDto(Guid id, DateTime takenOn, byte[] data, bool isFavourite)
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
