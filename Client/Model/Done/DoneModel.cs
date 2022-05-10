namespace Couple.Client.Model.Done;

public class DoneModel
{
    public DoneModel(DateOnly date, int largestOrder, int count)
    {
        (Date, LargestOrder, Count) = (date, largestOrder, count);
    }

    public DateOnly Date { get; }
    public int LargestOrder { get; set; }
    public int Count { get; set; }
}