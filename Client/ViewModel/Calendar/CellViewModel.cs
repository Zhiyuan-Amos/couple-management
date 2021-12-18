namespace Couple.Client.ViewModel.Calendar;

public class CellViewModel
{
    public CellViewModel(DateTime date, bool isThisMonth) => (Date, IsThisMonth) = (date, isThisMonth);

    public DateTime Date { get; }
    public bool IsThisMonth { get; }
}
