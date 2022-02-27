namespace Couple.Client.States.Image;

public class CreateUpdateImageStateContainer
{
    public CreateUpdateImageStateContainer()
    {
        DateTime = DateTime.Now;
        IsFavourite = false;
    }

    public CreateUpdateImageStateContainer(DateTime dateTime, bool isFavourite, byte[] data)
    {
        DateTime = dateTime;
        IsFavourite = isFavourite;
        Data = data;
    }

    public DateTime DateTime { get; set; }
    public bool IsFavourite { get; set; }
    public byte[] Data { get; set; }
}
