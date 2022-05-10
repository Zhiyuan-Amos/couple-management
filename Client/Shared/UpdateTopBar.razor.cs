using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Shared;

public partial class UpdateTopBar
{
    [Inject] private IJSRuntime Js { get; init; } = default!;
    [EditorRequired][Parameter] public string Title { get; init; } = default!;
    [EditorRequired][Parameter] public Func<Task> OnDeleteCallback { get; set; } = default!;

    private async Task Cancel() => await Js.InvokeVoidAsync("navigateBack");
}
