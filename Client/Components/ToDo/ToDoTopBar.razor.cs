using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.ToDo
{
    public partial class ToDoTopBar
    {
        [Parameter]
        public string SelectedCategory { get; set; }

        [Parameter]
        public EventCallback OnClickCallback { get; set; }

        [Parameter]
        public bool IsDropDown { get; set; }

        [Parameter]
        public EventCallback OnSynchronisationCallback { get; set; }
    }
}
