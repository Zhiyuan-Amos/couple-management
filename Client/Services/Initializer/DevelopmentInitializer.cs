using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.Services.Initializer;

public class DevelopmentInitializer : Initializer
{
    private static bool s_isDataLoaded;
    private readonly Synchronizer.Synchronizer _synchronizer;

    public DevelopmentInitializer(Synchronizer.Synchronizer synchronizer) => _synchronizer = synchronizer;

    public async Task InitializeAsync(Task<AuthenticationState> AuthenticationStateTask)
    {
        if (!s_isDataLoaded)
        {
            s_isDataLoaded = true;
            await _synchronizer.SynchronizeAsync();
        }
    }
}
