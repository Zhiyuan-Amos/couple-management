using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class AnimatedCategoryListView
    {
        [Parameter] public EventCallback<string> OnClickCallback { get; init; }

        private TelerikAnimationContainer CategoryAnimationContainer { get; set; }

        public Task ToggleAsync() => CategoryAnimationContainer.ToggleAsync();
        public Task HideAsync() => CategoryAnimationContainer.HideAsync();
    }
}
