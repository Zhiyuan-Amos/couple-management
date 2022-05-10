namespace Couple.Client.Shared;

public class UpdateTopBar
{
    [Inject] private IJSRuntime Js { get; init; } = default!;
    [EditorRequired] [Parameter] public string Title { get; init; } = default!;
    [EditorRequired] [Parameter] public Func<Task> OnDeleteCallback { get; set; } = default!;

    private async Task Cancel()
    {
        return await Js.InvokeVoidAsync("navigateBack");
    }
}