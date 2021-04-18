using Couple.Client.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Couple.Client.Pages.Calendar
{
    public partial class Calendar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public DateTime Selected { get; set; }

        private bool IsListViewExpanded { get; set; }
        private bool IsInitialStyle { get; set; } = true;

        private string CollapsedTableStyle => $"max-height: {CollapsedTableHeight}px; " +
                                              "overflow-y: hidden; " +
                                              "transition: 0.2s; " +
                                              "transition-timing-function: linear;";

        private string ExpandedTableStyle => $"max-height: {ExpandedTableHeight}px; " +
                                             "overflow-y: hidden; " +
                                             "transition: 0.2s; " +
                                             "transition-timing-function: linear;";

        private double CollapsedTableHeight { get; set; }
        private double ExpandedTableHeight { get; set; }

        private string InitialStyle => $"max-height: {ExpandedTableHeight}px;";

        protected override void OnInitialized()
        {
            if (Selected == new DateTime())
            {
                Selected = DateTime.Now.Date;
                NavigationManager.NavigateTo($"/calendar/{Selected.ToCalendarUrl()}");
            }
        }

        private void SetCalendarHeight(double collapsedHeight, double expandedHeight)
        {
            CollapsedTableHeight = collapsedHeight;
            ExpandedTableHeight = expandedHeight;
            StateHasChanged();
        }

        private string GetCalendarStyle()
        {
            if (IsInitialStyle)
            {
                return InitialStyle;
            }

            return IsListViewExpanded ? CollapsedTableStyle : ExpandedTableStyle;
        }

        private string GetListStyle() => IsListViewExpanded
            ? $"min-height: calc(100% - {CollapsedTableHeight}px); max-height: calc(100% - {CollapsedTableHeight}px); overflow-y: scroll; transition: 0.2s; transition-timing-function: linear;"
            : $"min-height: calc(100% - {ExpandedTableHeight}px); max-height: calc(100% - {ExpandedTableHeight}px); overflow-y: hidden; transition: 0.2s; transition-timing-function: linear;";


        private void SwipeUp()
        {
            IsInitialStyle = false;

            if (!IsListViewExpanded)
            {
                IsListViewExpanded = true;
            }

            StateHasChanged();
        }

        private void SwipeDown()
        {
            IsInitialStyle = false;

            if (IsListViewExpanded && ((IJSInProcessRuntime) Js).Invoke<bool>("isListViewTop"))
            {
                IsListViewExpanded = false;
            }

            StateHasChanged();
        }

        private void AddEvent() => NavigationManager.NavigateTo($"/calendar/create");
    }
}
