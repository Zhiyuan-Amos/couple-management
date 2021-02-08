using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Couple.Client.Components.ToDo
{
    public partial class AnimatedCategoryListView
    {
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }

        private TelerikAnimationContainer CategoryAnimationContainer { get; set; }

        public Task ToggleAsync() => CategoryAnimationContainer.ToggleAsync();
        public Task HideAsync() => CategoryAnimationContainer.HideAsync();
    }
}
