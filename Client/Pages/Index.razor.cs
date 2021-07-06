using Couple.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Couple.Client.Pages
{
    public partial class Index
    {
        [Inject] private Synchronizer Synchronizer { get; init; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
            }
        }
    }
}
