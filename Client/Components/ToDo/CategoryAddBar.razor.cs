using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.ToDo
{
    public partial class CategoryAddBar
    {
        protected string Category { get; set; }

        [Parameter]
        public EventCallback<string> OnAddCallback { get; set; }
    }
}
