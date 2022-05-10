using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Shared;

public partial class BaseTopBar
{
    [EditorRequired][Parameter] public RenderFragment Content { get; init; } = default!;

    [Inject] private Synchronizer Synchronizer { get; init; } = default!;

    private async Task Synchronize() => await Synchronizer.SynchronizeAsync();
}
