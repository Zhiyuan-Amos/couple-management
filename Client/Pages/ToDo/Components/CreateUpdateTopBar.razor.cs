using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CreateUpdateTopBar
    {
        [Inject]
        protected NavigationManager NavigationManager { get; init; }

        [Parameter]
        public EventCallback OnClickCallback { get; init; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public bool IsEnabled { get; set; }

        protected void Cancel() => NavigationManager.NavigateTo("/todo");
    }
}