using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Shared;

public partial class MainLayout
{
    [Inject] private Synchronizer Synchronizer { get; init; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Synchronizer.SynchronizeAsync();
        }
    }
}
