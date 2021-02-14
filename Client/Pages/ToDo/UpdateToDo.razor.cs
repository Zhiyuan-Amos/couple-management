using Couple.Client.Components.ToDo;
using Couple.Client.Data.ToDo;
using Couple.Client.States.ToDo;
using Couple.Shared.Model.ToDo;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public class UpdateToDoBase : CreateUpdateToDoBase
    {
        [Parameter]
        public Guid ToDoId { get; set; }

        protected AnimatedCategoryListViewWithAdd AnimatedCategorySelectionListView { get; set; }

        protected UpdateToDoViewModel ToUpdate { get; set; } = new();

        protected override void OnInitialized()
        {
            if (!ToDoStateContainer.TryGetToDo(ToDoId, out var toDo))
            {
                NavigationManager.NavigateTo("todo");
                return;
            }

            ToUpdate = new()
            {
                Id = toDo.Id,
                Text = toDo.Text,
                Category = toDo.Category,
                CreatedOn = toDo.CreatedOn,
            };
        }

        protected async Task Delete()
        {
            await LocalStore.DeleteAsync("todo", ToUpdate.Id);
            var toDos = await LocalStore.GetAllAsync<List<ToDoModel>>("todo");
            ToDoStateContainer.ToDos = toDos;

            NavigationManager.NavigateTo("/todo");

            await HttpClient.DeleteAsync($"api/ToDos/{ToUpdate.Id}");
        }

        protected override async Task Save()
        {
            var toPersist = new ToDoModel
            {
                Id = ToUpdate.Id,
                Text = ToUpdate.Text,
                Category = ToUpdate.Category,
                CreatedOn = ToUpdate.CreatedOn,
            };
            await LocalStore.PutAsync("todo", toPersist);

            var toDos = await LocalStore.GetAllAsync<List<ToDoModel>>("todo");
            ToDoStateContainer.ToDos = toDos;
            SelectedCategoryStateContainer.SelectedCategory = ToUpdate.Category;
            NavigationManager.NavigateTo("/todo");

            var toUpdate = new UpdateToDoDto
            {
                Id = ToUpdate.Id,
                Text = ToUpdate.Text,
                Category = ToUpdate.Category,
                CreatedOn = ToUpdate.CreatedOn,
            };
            await HttpClient.PutAsJsonAsync($"api/ToDos", toUpdate);
        }

        protected override async Task Select(string category)
        {
            await AnimatedCategorySelectionListView.HideAsync();
            ToUpdate.Category = category;
        }

        protected override async Task ShowSelectionWindow()
        {
            await AnimatedCategorySelectionListView.ShowAsync();
        }

        protected override async Task CancelSelection()
        {
            await AnimatedCategorySelectionListView.HideAsync();
        }

        protected override bool IsEnabled => !string.IsNullOrWhiteSpace(ToUpdate?.Text) && !string.IsNullOrWhiteSpace(ToUpdate?.Category);

        protected class UpdateToDoViewModel
        {
            public Guid Id { get; set; }
            public string Text { get; set; }
            public string Category { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}
