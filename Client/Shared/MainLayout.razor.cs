using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.Shared;

public partial class MainLayout
{
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private Synchronizer Synchronizer { get; init; } = default!;

    protected override async Task OnInitializedAsync()
    {
        // Should have done https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/?view=aspnetcore-6.0#require-authorization-for-the-entire-app,
        // but that doesn't work
        var authenticationState = await AuthenticationStateTask;
        if (authenticationState.User.Identity is null || !authenticationState.User.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo(
                $"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var authenticationState = await AuthenticationStateTask;
        if (firstRender && authenticationState.User.Identity is not null &&
            authenticationState.User.Identity.IsAuthenticated)
        {
            await Synchronizer.SynchronizeAsync();
        }
    }
}
