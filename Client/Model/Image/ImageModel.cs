using System;

namespace Couple.Client.Model.Image;

public class ImageModel
{
    public Guid Id { get; }
    public DateTime TakenOn { get; }
    public byte[] Data { get; }
    public bool IsFavourite { get; }

    public ImageModel(Guid id, DateTime takenOn, byte[] data, bool isFavourite) =>
        (Id, TakenOn, Data, IsFavourite) = (id, takenOn, data, isFavourite);
}
