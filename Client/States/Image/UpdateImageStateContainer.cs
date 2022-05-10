namespace Couple.Client.States.Image;

public class UpdateImageStateContainer
{
    public UpdateImageStateContainer(DateTime dateTime, bool isFavourite, byte[] data)
    {
        DateTime = dateTime;
        IsFavourite = isFavourite;
        Data = data;
    }

    public DateTime DateTime { get; set; }
    public bool IsFavourite { get; set; }
    public byte[]? Data { get; set; }
}
