using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class ToDoTopBar
    {
        [Parameter]
        public string SelectedCategory { get; set; }

        [Parameter]
        public EventCallback OnClickCallback { get; init; }

        [Parameter]
        public bool IsDropDown { get; set; }

        [Parameter]
        public EventCallback OnSynchronisationCallback { get; init; }
    }
}
