namespace Couple.Client.Shared;

public partial class CreateTopBar
{
    [EditorRequired] [Parameter] public string Title { get; init; } = default!;
    [Inject] private IJSRuntime Js { get; } = default!;

    private async Task Cancel() => await Js.InvokeVoidAsync("navigateBack");
}
