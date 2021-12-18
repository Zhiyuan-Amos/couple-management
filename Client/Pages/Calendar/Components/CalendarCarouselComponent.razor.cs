using Couple.Client.States.Calendar;
using Couple.Client.Utility;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Calendar.Components;

public partial class CalendarCarouselComponent
{
    private const int WeeksOnCalendar = 6;

    private const int DaysOnCalendar = 42;

    private HorizontalSwipeHandler _horizontalSwipeHandler;
    [Parameter] public string Style { get; set; }
    [Parameter] public Action<double, double> AfterRenderCallback { get; init; }

    [Inject] private SelectedDateStateContainer SelectedDateStateContainer { get; init; }
    [Inject] private IJSRuntime Js { get; init; }

    protected override void OnInitialized()
    {
        SelectedDateStateContainer.SelectedDate = DateTime.Today;
        _horizontalSwipeHandler = new(SwipeLeft, SwipeRight);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            ((IJSInProcessRuntime)Js).InvokeVoid("initializeCarousel");
            var calendarBodyHeight = ((IJSInProcessRuntime)Js).Invoke<double>("getCalendarBodyHeight");

            AfterRenderCallback.Invoke(calendarBodyHeight / WeeksOnCalendar, calendarBodyHeight);
        }
    }

    private static List<CellViewModel> GetCells(int year, int month)
    {
        var firstDayOfMonth = new DateTime(year, month, 1);
        var firstDayOnCalendar = firstDayOfMonth.AddDays((int)firstDayOfMonth.DayOfWeek * -1);

        return Enumerable.Range(0, DaysOnCalendar)
            .Select(i => firstDayOnCalendar.AddDays(i))
            .Select(date => new CellViewModel(date, date.Month == month))
            .ToList();
    }

    private void SwipeLeft() => ((IJSInProcessRuntime)Js).InvokeVoid("next");

    private void SwipeRight() => ((IJSInProcessRuntime)Js).InvokeVoid("prev");
}
