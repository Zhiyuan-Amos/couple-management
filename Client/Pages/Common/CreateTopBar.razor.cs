using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Common
{
    public partial class CreateTopBar
    {
        [Parameter] public string Title { get; init; }
        [Parameter] public string OnCancelUrl { get; init; }
        [Inject] private NavigationManager NavigationManager { get; init; }

        private void Cancel() => NavigationManager.NavigateTo(OnCancelUrl);
    }
}
