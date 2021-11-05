using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Couple.Client.Shared
{
    public partial class UpdateTopBar
    {
        [Inject] private IJSRuntime Js { get; init; }
        [Parameter] public string Title { get; init; }
        [Parameter] public Func<Task> OnDeleteCallback { get; set; }

        private async Task Cancel() => await Js.InvokeVoidAsync("navigateBack");
    }
}
