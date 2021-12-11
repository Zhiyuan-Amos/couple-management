namespace Couple.Client.States.Image;

public class CreateUpdateImageStateContainer
{
    public CreateUpdateImageStateContainer()
    {
        IsFavourite = false;
    }

    public CreateUpdateImageStateContainer(bool isFavourite, byte[] data)
    {
        IsFavourite = isFavourite;
        Data = data;
    }

    public bool IsFavourite { get; set; }
    public byte[] Data { get; set; }
}
