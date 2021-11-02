using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Common
{
    public partial class UpdateTopBar
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Parameter] public string Title { get; init; }
        [Parameter] public string OnCancelUrl { get; init; }
        [Parameter] public Func<Task> OnDeleteCallback { get; set; }

        private void Cancel() => NavigationManager.NavigateTo(OnCancelUrl);
    }
}
