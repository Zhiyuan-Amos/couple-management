using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class UpdateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Parameter] public Func<Task> OnDeleteCallback { get; set; }

        private void Cancel() => NavigationManager.NavigateTo("/todo");
    }
}
