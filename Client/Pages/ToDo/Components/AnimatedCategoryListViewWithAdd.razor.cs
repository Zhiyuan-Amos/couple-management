﻿using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class AnimatedCategoryListViewWithAdd
    {
        [Parameter] public EventCallback<string> OnConfirmCallback { get; init; }

        [Parameter] public EventCallback OnCancelCallback { get; init; }

        private TelerikAnimationContainer CategoryAnimationContainer { get; set; }

        private async Task Confirm(string category) => await OnConfirmCallback.InvokeAsync(category);
        private async Task Cancel() => await OnCancelCallback.InvokeAsync();

        public Task ShowAsync() => CategoryAnimationContainer.ShowAsync();
        public Task HideAsync() => CategoryAnimationContainer.HideAsync();
    }
}
