using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Image.Components
{
    public partial class UpdateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Parameter] public Func<Task> OnDeleteCallback { get; set; }

        private void Cancel() => NavigationManager.NavigateTo("/done");
    }
}
