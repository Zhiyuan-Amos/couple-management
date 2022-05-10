using Couple.Client.Services.Synchronizer;

namespace Couple.Client.Shared;

public class BaseTopBar
{
    [EditorRequired] [Parameter] public RenderFragment Content { get; init; } = default!;

    [Inject] private Synchronizer Synchronizer { get; init; } = default!;

    private async Task Synchronize()
    {
        return await Synchronizer.SynchronizeAsync();
    }
}