using Couple.Client.Services.Settings;
using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Couple.Client.Shared;

public partial class MainLayout
{
    private static bool s_isDataLoaded;
    [Inject] private Synchronizer Synchronizer { get; init; } = default!;
    [Inject] private IJSRuntime Js { get; init; } = default!;
    [Inject] private IOptions<AuthenticationOptions> AuthOptions { get; init; } = default!;


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var ao = AuthOptions.Value;
        // This seems necessary as https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-net-aad-b2c-considerations#acquire-a-token-to-apply-a-policy doesn't work
        await Js.InvokeVoidAsync("login", ao.ClientId, ao.Authority, ao.KnownAuthority, ao.RedirectUri);

        if (!s_isDataLoaded)
        {
            s_isDataLoaded = true;
            await Synchronizer.SynchronizeAsync();
        }
    }
}
