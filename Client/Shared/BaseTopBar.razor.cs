using Couple.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Couple.Client.Shared
{
    public partial class BaseTopBar
    {
        [Parameter]
        public RenderFragment Content { get; init; }

        [Parameter]
        public EventCallback OnSynchronisationCallback { get; init; }

        [Inject]
        private Synchronizer Synchronizer { get; init; }

        protected Task Synchronize() => Synchronizer.SynchronizeAsync();
    }
}
