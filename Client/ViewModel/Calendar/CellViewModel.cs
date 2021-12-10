namespace Couple.Client.ViewModel.Calendar;

public class CellViewModel
{
    public DateTime Date { get; }
    public bool IsThisMonth { get; }

    public CellViewModel(DateTime date, bool isThisMonth)
        => (Date, IsThisMonth) = (date, isThisMonth);
}
