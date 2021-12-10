using System;

namespace Couple.Shared.Model.Image;

public class ImageDto
{
    public Guid Id { get; }
    public byte[] Data { get; }

    public ImageDto(Guid id, byte[] data)
    {
        Id = id;
        Data = data;
    }
}
