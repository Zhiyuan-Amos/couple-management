using Couple.Client.Model.ToDo;
using Couple.Client.Pages.ToDo.Components;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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

        protected UpdateToDoViewModel ToUpdate { get; set; }

        private IJSObjectReference _module;

        protected override async Task OnInitializedAsync()
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
            _module = await Js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
        }

        protected async Task Delete()
        {
            await _module.InvokeVoidAsync("remove", ToUpdate.Id);
            var toDos = await _module.InvokeAsync<List<ToDoModel>>("getAll");
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
            await _module.InvokeVoidAsync("update", toPersist);

            var toDos = await _module.InvokeAsync<List<ToDoModel>>("getAll");
            ToDoStateContainer.ToDos = toDos;
            SelectedCategoryStateContainer.SelectedCategory = ToUpdate.Category;
            NavigationManager.NavigateTo("/todo");

            var toUpdate = Mapper.Map<UpdateToDoDto>(toPersist);
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
    }
}
