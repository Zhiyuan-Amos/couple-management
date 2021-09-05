using Couple.Client.Services.Synchronizer;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Couple.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] private Synchronizer Synchronizer { get; init; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Synchronizer.SynchronizeAsync();
            }
        }
    }
}
