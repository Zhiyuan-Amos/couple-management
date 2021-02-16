using Couple.Client.Utility;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Couple.Client.Shared
{
    public partial class BaseTopBar
    {
        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public EventCallback OnSynchronisationCallback { get; set; }

        [Inject]
        public Synchronizer Synchronizer { get; set; }

        protected async Task Synchronize()
        {
            await Synchronizer.SynchronizeAsync();
            await OnSynchronisationCallback.InvokeAsync();
        }
    }
}
