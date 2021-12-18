using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Shared;

public partial class CreateTopBar
{
    [EditorRequired] [Parameter] public string Title { get; init; }
    [Inject] private IJSRuntime Js { get; init; }

    private async Task Cancel() => await Js.InvokeVoidAsync("navigateBack");
}
