using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Shared;

public partial class UpdateTopBar
{
    [Inject] private IJSRuntime Js { get; init; }
    [EditorRequired] [Parameter] public string Title { get; init; }
    [EditorRequired] [Parameter] public Func<Task> OnDeleteCallback { get; set; }

    private async Task Cancel()
    {
        await Js.InvokeVoidAsync("navigateBack");
    }
}
