using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.Services.Initializer;

public interface Initializer
{
    Task InitializeAsync(Task<AuthenticationState> authenticationStateTask);
}
