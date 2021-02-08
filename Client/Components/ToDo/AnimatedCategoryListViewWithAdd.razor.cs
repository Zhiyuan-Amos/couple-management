using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Couple.Client.Components.ToDo
{
    public partial class AnimatedCategoryListViewWithAdd
    {
        [Parameter]
        public EventCallback<string> OnConfirmCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        private TelerikAnimationContainer CategoryAnimationContainer { get; set; }

        protected async Task Confirm(string category) => await OnConfirmCallback.InvokeAsync(category);
        protected async Task Cancel() => await OnCancelCallback.InvokeAsync();

        public Task ShowAsync() => CategoryAnimationContainer.ShowAsync();
        public Task HideAsync() => CategoryAnimationContainer.HideAsync();
    }
}
