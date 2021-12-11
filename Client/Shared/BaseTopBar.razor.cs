using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Shared;

public partial class BaseTopBar
{
    [Parameter] public RenderFragment Content { get; init; }

    [Parameter] public EventCallback OnSynchronisationCallback { get; init; }

    [Inject] private Synchronizer Synchronizer { get; init; }

    private Task Synchronize()
    {
        return Synchronizer.SynchronizeAsync();
    }
}
