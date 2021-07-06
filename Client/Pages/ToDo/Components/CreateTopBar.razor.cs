using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CreateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        private void Cancel() => NavigationManager.NavigateTo("/todo");
    }
}
