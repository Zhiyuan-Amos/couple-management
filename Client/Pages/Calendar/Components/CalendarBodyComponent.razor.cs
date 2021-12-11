using Couple.Client.States.Calendar;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Calendar.Components;

public partial class CalendarBodyComponent
{
    private const int WeeksOnCalendar = 6;
    private const int DaysOfWeek = 7;

    private DateTime _selected;
    [Inject] private SelectedDateStateContainer SelectedDateStateContainer { get; init; }

    [Parameter] public List<CellViewModel> Cells { get; set; }

    public void Dispose()
    {
        SelectedDateStateContainer.OnChange -= SelectedDateChangedHandler;
    }

    protected override void OnInitialized()
    {
        _selected = SelectedDateStateContainer.SelectedDate;
        SelectedDateStateContainer.OnChange += SelectedDateChangedHandler;
    }

    private void SelectionHandler(DateTime selected)
    {
        SelectedDateStateContainer.SelectedDate = selected;
    }

    private void SelectedDateChangedHandler()
    {
        _selected = SelectedDateStateContainer.SelectedDate;
    }
}
