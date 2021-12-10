using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Calendar;

public partial class Calendar
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IJSRuntime Js { get; init; }

    private bool IsListViewExpanded { get; set; }

    private bool HasExpanded { get; set; }
    private double HeaderHeight { get; set; }
    private double CollapsedBodyHeight { get; set; }
    private double ExpandedBodyHeight { get; set; }

    // Does not have `transition` property so that the calendar appears without transition on page load
    private string InitialBodyStyle => $"max-height: {ExpandedBodyHeight}px;";

    private string CollapsedBodyStyle => $"max-height: {CollapsedBodyHeight}px; " +
                                         "overflow-y: hidden; " +
                                         "transition: 0.2s; " +
                                         "transition-timing-function: linear;";

    private string ExpandedBodyStyle => $"max-height: {ExpandedBodyHeight}px; " +
                                        "overflow-y: hidden; " +
                                        "transition: 0.2s; " +
                                        "transition-timing-function: linear;";

    private void SetCalendarHeaderHeight(double headerHeight)
    {
        HeaderHeight = headerHeight;
        StateHasChanged();
    }

    private void SetCalendarBodyHeight(double collapsedBodyHeight, double expandedBodyHeight)
    {
        CollapsedBodyHeight = collapsedBodyHeight;
        ExpandedBodyHeight = expandedBodyHeight;
        StateHasChanged();
    }

    private string GetCalendarStyle()
    {
        if (!HasExpanded)
        {
            return InitialBodyStyle;
        }

        return IsListViewExpanded ? CollapsedBodyStyle : ExpandedBodyStyle;
    }

    private string GetListStyle()
    {
        if (IsListViewExpanded)
        {
            var calendarHeight = HeaderHeight + CollapsedBodyHeight;
            return $"min-height: calc(100% - {calendarHeight}px); max-height: calc(100% - {calendarHeight}px); " +
                   "overflow-y: scroll; transition: 0.2s; transition-timing-function: linear;";
        }
        else
        {
            var calendarHeight = HeaderHeight + ExpandedBodyHeight;
            return $"min-height: calc(100% - {calendarHeight}px); max-height: calc(100% - {calendarHeight}px); " +
                   "overflow-y: hidden; transition: 0.2s; transition-timing-function: linear;";
        }
    }


    private void SwipeUp()
    {
        HasExpanded = true;

        if (!IsListViewExpanded)
        {
            IsListViewExpanded = true;
        }

        StateHasChanged();
    }

    private void SwipeDown()
    {
        if (IsListViewExpanded && ((IJSInProcessRuntime)Js).Invoke<bool>("isListViewTop"))
        {
            IsListViewExpanded = false;
        }

        StateHasChanged();
    }

    private void AddEvent() => NavigationManager.NavigateTo($"/calendar/create");
}
