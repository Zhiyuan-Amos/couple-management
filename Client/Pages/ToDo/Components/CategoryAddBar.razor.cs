using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CategoryAddBar
    {
        protected string Category { get; set; }

        [Parameter]
        public EventCallback<string> OnAddCallback { get; set; }
    }
}
