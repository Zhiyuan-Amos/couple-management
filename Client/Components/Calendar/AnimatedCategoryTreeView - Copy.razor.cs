using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Couple.Client.Components.Calendar
{
    public partial class AnimatedCategoryTreeView
    {
        private TelerikAnimationContainer CategoryAnimationContainer { get; set; }

        [Parameter]
        public List<ToDoViewModel> Added { get; set; }

        [Parameter]
        public EventCallback<List<ToDoViewModel>> AddedChanged { get; set; }

        [Parameter]
        public List<ToDoViewModel> Removed { get; set; }

        public List<ToDoViewModel> Selected { get; set; }

        [Parameter]
        public EventCallback OnCloseCallback { get; set; }

        protected override void OnInitialized()
        {
            Selected = new();
        }

        public Task ShowAsync() => CategoryAnimationContainer.ShowAsync();
        public Task HideAsync() => CategoryAnimationContainer.HideAsync();

        protected async Task Save()
        {
            await AddedChanged.InvokeAsync(Selected);
            await OnCloseCallback.InvokeAsync();
        }
    }
}
