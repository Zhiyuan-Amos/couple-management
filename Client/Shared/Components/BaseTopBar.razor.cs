using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Shared.Components;

public partial class BaseTopBar
{
    [EditorRequired][Parameter] public RenderFragment Content { get; init; } = default!;

    [Inject] private Synchronizer Synchronizer { get; init; } = default!;

    private async Task Synchronize() => await Synchronizer.SynchronizeAsync();
}
