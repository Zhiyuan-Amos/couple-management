using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.Calendar
{
    public partial class CategoryTreeViewTopBar
    {
        [Parameter]
        public EventCallback OnCrossCallback { get; set; }

        [Parameter]
        public EventCallback OnSaveCallback { get; set; }
    }
}
