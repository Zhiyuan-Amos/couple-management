namespace Couple.Shared.Model.Image;

public class ImageDto
{
    public ImageDto(string id, byte[] data)
    {
        Id = id;
        Data = data;
    }

    public string Id { get; }
    public byte[] Data { get; }
}
