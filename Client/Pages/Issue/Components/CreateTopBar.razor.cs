using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Issue.Components
{
    public partial class CreateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        private void Cancel() => NavigationManager.NavigateTo("/todo");
    }
}
