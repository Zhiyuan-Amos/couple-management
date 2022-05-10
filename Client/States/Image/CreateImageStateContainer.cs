namespace Couple.Client.States.Image;

public class CreateImageStateContainer
{
    public CreateImageStateContainer()
    {
        DateTime = DateTime.Now;
        IsFavourite = false;
        Data = new();
    }

    public DateTime DateTime { get; set; }
    public bool IsFavourite { get; set; }
    public List<byte[]> Data { get; }
}
