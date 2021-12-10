using Couple.Client.Model.Issue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couple.Client.ViewModel.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Calendar.Components;

public partial class CreateUpdateForm
{
    [Inject] private IJSRuntime Js { get; init; }

    [Parameter] public string Title { get; set; }

    [Parameter] public EventCallback<string> TitleChanged { get; init; }

    [Parameter] public DateTime Start { get; set; }

    [Parameter] public EventCallback<DateTime> StartChanged { get; init; }

    [Parameter] public DateTime End { get; set; }

    [Parameter] public EventCallback<DateTime> EndChanged { get; init; }

    [Parameter] public List<IssueModel> Added { get; set; }

    [Parameter] public EventCallback<List<IssueModel>> AddedChanged { get; init; }

    [Parameter] public List<IssueModel> Removed { get; set; }

    [Parameter] public EventCallback<IssueModel> RemovedChanged { get; init; }

    [Parameter] public List<IssueModel> Total { get; set; }

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

    private Task Remove(IssueModel toRemove) => RemovedChanged.InvokeAsync(toRemove);
}
