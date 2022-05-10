using Couple.Client.Services.Synchronizer;

namespace Couple.Client.Shared;

public partial class BaseTopBar
{
    [EditorRequired] [Parameter] public RenderFragment Content { get; init; } = default!;

    [Inject] private Synchronizer Synchronizer { get; } = default!;

    private async Task Synchronize() => await Synchronizer.SynchronizeAsync();
}
