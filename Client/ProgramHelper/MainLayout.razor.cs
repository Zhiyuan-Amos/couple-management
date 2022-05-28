using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Couple.Client.ProgramHelper;

public partial class MainLayout
{
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;
    [Inject] private Initializer Initializer { get; init; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender) => await Initializer.InitializeAsync(AuthenticationStateTask);
}
