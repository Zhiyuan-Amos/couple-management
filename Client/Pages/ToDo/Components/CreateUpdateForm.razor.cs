using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class CreateUpdateForm
    {
        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public EventCallback<string> TextChanged { get; init; }

        [Parameter]
        public string Category { get; set; }

        [Parameter]
        public EventCallback OnClickCallback { get; init; }
    }
}
