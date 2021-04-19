using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CategoryAddBar
    {
        [Parameter] public EventCallback<string> OnAddCallback { get; init; }

        private string Category { get; set; }
    }
}
