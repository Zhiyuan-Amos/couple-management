using Couple.Client.Components.ToDo;
using Couple.Client.Data.ToDo;
using Couple.Client.States.ToDo;
using Couple.Shared.Model.ToDo;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public class CreateToDoBase : CreateUpdateToDoBase
    {
        protected AnimatedCategoryListViewWithAdd AnimatedCategorySelectionListView { get; set; }

        protected CreateToDoViewModel ToCreate { get; set; } = new();

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
            await LocalStore.PutAsync("todo", toPersist);

            await ToDoStateContainer.RefreshAsync();
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
