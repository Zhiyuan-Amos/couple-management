namespace Couple.Client.States.Image;

public class CreateUpdateImageStateContainer
{
    public CreateUpdateImageStateContainer()
    {
        Date = DateOnly.FromDateTime(DateTime.Now);
        IsFavourite = false;
    }

    public CreateUpdateImageStateContainer(DateOnly date, bool isFavourite, byte[] data)
    {
        Date = date;
        IsFavourite = isFavourite;
        Data = data;
    }

    public DateOnly Date { get; set; }
    public bool IsFavourite { get; set; }
    public byte[] Data { get; set; }
}
