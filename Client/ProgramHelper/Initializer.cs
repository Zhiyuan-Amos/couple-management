using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.ProgramHelper;

public interface Initializer
{
    Task InitializeAsync(Task<AuthenticationState> authenticationStateTask);
}
