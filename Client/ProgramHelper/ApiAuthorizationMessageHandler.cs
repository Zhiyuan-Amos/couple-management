using Couple.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Couple.Client.ProgramHelper;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager,
        IConfiguration configuration)
        : base(provider, navigationManager) =>
        ConfigureHandler(
            new[] { configuration[Constants.ApiPrefix]! },
            new[] { Constants.Scope });
}
