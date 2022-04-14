using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.Shared;

public partial class MainLayout
{
    private static bool s_isDataLoaded;
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;
    [Inject] private Synchronizer Synchronizer { get; init; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var authenticationState = await AuthenticationStateTask;
        if (!s_isDataLoaded && authenticationState.User.Identity is not null &&
            authenticationState.User.Identity.IsAuthenticated)
        {
            s_isDataLoaded = true;
            await Synchronizer.SynchronizeAsync();
        }
    }
}
