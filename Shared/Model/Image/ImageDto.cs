namespace Couple.Shared.Model.Image;

public class ImageDto
{
    public ImageDto(Guid id, byte[] data)
    {
        Id = id;
        Data = data;
    }

    public Guid Id { get; }
    public byte[] Data { get; }
}
