using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CategoryTopBar
    {
        [Parameter]
        public EventCallback OnCancelCallback { get; set; }
    }
}
