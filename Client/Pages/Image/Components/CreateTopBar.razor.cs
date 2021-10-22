using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Image.Components
{
    public partial class CreateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        private void Cancel() => NavigationManager.NavigateTo("/settings");
    }
}
