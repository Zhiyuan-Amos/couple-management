using Couple.Client.Model.ToDo;
using Couple.Client.Pages.ToDo.Components;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Couple.Shared.Model.ToDo;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public class CreateToDoBase : CreateUpdateToDoBase
    {
        protected AnimatedCategoryListViewWithAdd AnimatedCategorySelectionListView { get; set; }

        protected CreateToDoViewModel ToCreate { get; set; } = new();

        private IJSObjectReference Module;

        protected override async Task OnInitializedAsync()
        {
            ToCreate = new()
            {
                Text = "",
                Category = SelectedCategoryStateContainer.SelectedCategory,
            };
            Module = await Js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
        }

        protected override async Task Save()
        {
            var id = Guid.NewGuid();
            var toPersist = new ToDoModel
            {
                Id = id,
                Text = ToCreate.Text,
                Category = ToCreate.Category,
                CreatedOn = DateTime.Now,
            };
            await Module.InvokeVoidAsync("add", toPersist);

            var toDos = await Module.InvokeAsync<List<ToDoModel>>("getAll");
            ToDoStateContainer.ToDos = toDos;
            SelectedCategoryStateContainer.SelectedCategory = ToCreate.Category;
            NavigationManager.NavigateTo("/todo");

            var toCreate = new CreateToDoDto
            {
                Id = id,
                Text = ToCreate.Text,
                Category = ToCreate.Category,
                CreatedOn = DateTime.Now,
            };
            await HttpClient.PostAsJsonAsync($"api/ToDos", toCreate);
        }

        protected override async Task Select(string category)
        {
            await AnimatedCategorySelectionListView.HideAsync();
            ToCreate.Category = category;
        }

        protected override async Task ShowSelectionWindow()
        {
            await AnimatedCategorySelectionListView.ShowAsync();
        }

        protected override async Task CancelSelection()
        {
            await AnimatedCategorySelectionListView.HideAsync();
        }

        protected override bool IsEnabled => !string.IsNullOrWhiteSpace(ToCreate?.Text) && !string.IsNullOrWhiteSpace(ToCreate?.Category);

        protected class CreateToDoViewModel
        {
            public string Text { get; set; }
            public string Category { get; set; }
        }
    }
}
