using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.ToDo
{
    public partial class CategoryTopBar
    {
        [Parameter]
        public EventCallback OnCancelCallback { get; set; }
    }
}
