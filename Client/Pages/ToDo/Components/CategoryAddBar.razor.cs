using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CategoryAddBar
    {
        [Parameter] public EventCallback<string> OnAddCallback { get; init; }

        protected string Category { get; set; }
    }
}
