using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.ProgramHelper;

public class DevelopmentInitializer : Initializer
{
    private static bool s_isDataLoaded;
    private readonly Synchronizer _synchronizer;

    public DevelopmentInitializer(Synchronizer synchronizer) => _synchronizer = synchronizer;

    public async Task InitializeAsync(Task<AuthenticationState> AuthenticationStateTask)
    {
        if (!s_isDataLoaded)
        {
            s_isDataLoaded = true;
            await _synchronizer.SynchronizeAsync();
        }
    }
}
