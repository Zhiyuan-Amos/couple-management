using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Issue.Components
{
    public partial class UpdateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Parameter] public Func<Task> OnDeleteCallback { get; set; }

        private void Cancel() => NavigationManager.NavigateTo("/todo");
    }
}
