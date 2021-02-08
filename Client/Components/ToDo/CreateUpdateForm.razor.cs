using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.ToDo
{
    public partial class CreateUpdateForm
    {
        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public EventCallback<string> TextChanged { get; set; }

        [Parameter]
        public string Category { get; set; }

        [Parameter]
        public EventCallback OnClickCallback { get; set; }
    }
}
