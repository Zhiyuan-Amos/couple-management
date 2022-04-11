using Couple.Client.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Couple.Client.Infrastructure;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public const string Scope = @"https://couplesg.onmicrosoft.com/api/all";

    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager,
        IConfiguration configuration)
        : base(provider, navigationManager) =>
        ConfigureHandler(
            new[] { configuration[Constants.ApiPrefix]! },
            new[] { Scope });
}
