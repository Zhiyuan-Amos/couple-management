using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Calendar.Components
{
    public partial class CreateUpdateForm
    {
        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public string Title { get; set; }

        [Parameter] public EventCallback<string> TitleChanged { get; init; }

        [Parameter] public DateTime Start { get; set; }

        [Parameter] public EventCallback<DateTime> StartChanged { get; init; }

        [Parameter] public DateTime End { get; set; }

        [Parameter] public EventCallback<DateTime> EndChanged { get; init; }

        [Parameter] public List<ToDoViewModel> Added { get; set; }

        [Parameter] public EventCallback<List<ToDoViewModel>> AddedChanged { get; init; }

        [Parameter] public List<ToDoViewModel> Removed { get; set; }

        [Parameter] public EventCallback<ToDoViewModel> RemovedChanged { get; init; }

        [Parameter] public List<ToDoViewModel> Total { get; set; }

        private AnimatedCategoryTreeView CategoryListView { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var disableStartInput = Js.InvokeVoidAsync("disableInput", StartWrapperRef).AsTask();
                var disableEndInput = Js.InvokeVoidAsync("disableInput", EndWrapperRef).AsTask();
                await Task.WhenAll(disableStartInput, disableEndInput);
            }
        }

        // https://feedback.telerik.com/blazor/1475760-dropdowns-and-date-time-pickers-open-the-dropdown-on-component-focus
        // Modified from the above link
        private ElementReference StartWrapperRef { get; set; }
        private bool StartDoubleClick { get; set; } = true;

        private async Task ToggleStartPicker()
        {
            StartDoubleClick = !StartDoubleClick;
            if (!StartDoubleClick)
            {
                await Js.InvokeVoidAsync("togglePicker", StartWrapperRef);
            }
        }

        private ElementReference EndWrapperRef { get; set; }
        private bool EndDoubleClick { get; set; } = true;

        private async Task ToggleEndPicker() // can't extract w/ ToggleStartPicker() as bool is value type
        {
            EndDoubleClick = !EndDoubleClick;
            if (!EndDoubleClick)
            {
                await Js.InvokeVoidAsync("togglePicker", EndWrapperRef);
            }
        }

        private async Task StartDateChanged(DateTime newStartDate)
        {
            if (End < newStartDate)
            {
                await EndChanged.InvokeAsync(newStartDate);
            }

            await StartChanged.InvokeAsync(newStartDate);
        }

        private async Task EndDateChanged(DateTime newEndDate)
        {
            if (newEndDate < Start)
            {
                await StartChanged.InvokeAsync(newEndDate);
            }

            await EndChanged.InvokeAsync(newEndDate);
        }

        private async Task ShowToDoSelection()
        {
            await CategoryListView.ShowAsync();
            ((IJSInProcessRuntime) Js).InvokeVoid("setScroll", false);
        }

        private async Task CloseToDoSelection()
        {
            await CategoryListView.HideAsync();
            ((IJSInProcessRuntime) Js).InvokeVoid("setScroll", true);
        }

        private Task Remove(ToDoViewModel toRemove) => RemovedChanged.InvokeAsync(toRemove);
    }
}
