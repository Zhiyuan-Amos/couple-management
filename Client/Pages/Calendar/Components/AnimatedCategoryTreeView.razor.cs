using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Couple.Client.Pages.Calendar.Components
{
    public partial class AnimatedCategoryTreeView
    {
        [Parameter] public List<ToDoViewModel> Added { get; set; }

        [Parameter] public EventCallback<List<ToDoViewModel>> AddedChanged { get; init; }

        [Parameter] public List<ToDoViewModel> Removed { get; set; }

        [Parameter] public EventCallback OnCloseCallback { get; init; }

        private TelerikAnimationContainer CategoryAnimationContainer { get; set; }

        private List<ToDoViewModel> Selected { get; set; }

        protected override void OnInitialized()
        {
            Selected = new();
        }

        public Task ShowAsync() => CategoryAnimationContainer.ShowAsync();
        public Task HideAsync() => CategoryAnimationContainer.HideAsync();

        private Task Save()
        {
            var addCallbackTask = AddedChanged.InvokeAsync(Selected);
            var closeCallbackTask = OnCloseCallback.InvokeAsync();
            return Task.WhenAll(addCallbackTask, closeCallbackTask);
        }
    }
}
