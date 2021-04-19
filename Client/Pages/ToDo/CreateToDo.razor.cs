using Couple.Client.Model.ToDo;
using Couple.Client.Pages.ToDo.Components;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model.ToDo;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public class CreateToDoBase : CreateUpdateToDoBase
    {
        protected AnimatedCategoryListViewWithAdd AnimatedCategorySelectionListView { get; set; }

        protected CreateToDoViewModel ToCreate { get; set; }

        protected override void OnInitialized()
        {
            ToCreate = new()
            {
                Text = "",
                Category = SelectedCategoryStateContainer.SelectedCategory,
            };
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
            await Js.InvokeVoidAsync("addToDo", toPersist);

            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getAllToDos");
            SelectedCategoryStateContainer.SelectedCategory = ToCreate.Category;
            NavigationManager.NavigateTo("/todo");

            var toCreate = Mapper.Map<CreateToDoDto>(toPersist);
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

        protected override bool IsEnabled => !string.IsNullOrWhiteSpace(ToCreate?.Text) &&
                                             !string.IsNullOrWhiteSpace(ToCreate?.Category);
    }
}
