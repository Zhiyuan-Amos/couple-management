using Couple.Client.Services.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Initializer;

public class ProductionInitializer : Initializer
{
    private static bool s_isDataLoaded;
    private readonly Synchronizer.Synchronizer _synchronizer;
    private readonly IJSRuntime _js;
    private readonly IOptions<AuthenticationOptions> _authOptions;

    public ProductionInitializer(Synchronizer.Synchronizer synchronizer,
        IJSRuntime js,
        IOptions<AuthenticationOptions> authOptions)
    {
        _synchronizer = synchronizer;
        _js = js;
        _authOptions = authOptions;
    }

    public async Task InitializeAsync(Task<AuthenticationState> authenticationStateTask)
    {
        var ao = _authOptions.Value;
        // This seems necessary as https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-net-aad-b2c-considerations#acquire-a-token-to-apply-a-policy doesn't work
        await _js.InvokeVoidAsync("login", ao.ClientId, ao.Authority, ao.KnownAuthority, ao.RedirectUri);

        // Ensures that background API calls are only done after authentication state has been set
        await authenticationStateTask;

        if (!s_isDataLoaded)
        {
            s_isDataLoaded = true;
            await _synchronizer.SynchronizeAsync();
        }
    }
}
