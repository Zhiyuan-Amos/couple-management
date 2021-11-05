using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Couple.Client.Shared
{
    public partial class CreateTopBar
    {
        [Parameter] public string Title { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        private async Task Cancel() => await Js.InvokeVoidAsync("navigateBack");
    }
}
