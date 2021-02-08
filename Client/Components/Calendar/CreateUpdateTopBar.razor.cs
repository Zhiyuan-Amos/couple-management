using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.Calendar
{
    public partial class CreateUpdateTopBar
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public EventCallback OnClickCallback { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public bool IsEnabled { get; set; }

        protected void Cancel() => NavigationManager.NavigateTo("/calendar");
    }
}
