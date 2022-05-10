namespace Couple.Client.Shared;

public class CreateTopBar
{
    [EditorRequired] [Parameter] public string Title { get; init; } = default!;
    [Inject] private IJSRuntime Js { get; init; } = default!;

    private async Task Cancel()
    {
        return await Js.InvokeVoidAsync("navigateBack");
    }
}