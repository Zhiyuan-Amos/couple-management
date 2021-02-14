using Couple.Client.Components.ToDo;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public partial class ToDo
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        private SelectedCategoryStateContainer SelectedCategoryStateContainer { get; set; }

        private AnimatedCategoryListView CategoryListView { get; set; }

        protected bool IsDropdown { get; set; }
        protected string SelectedCategory { get; set; }
        protected List<ToDoViewModel> ToDos { get; set; }

        protected override void OnInitialized()
        {
            RefreshData();
        }

        protected void AddToDo() => NavigationManager.NavigateTo($"/todo/create");

        protected async Task ToggleVisibility()
        {
            IsDropdown = !IsDropdown;
            await CategoryListView.ToggleAsync();
            await ((IJSInProcessRuntime)Js).InvokeVoidAsync("toggleScroll");
        }

        protected async Task Select(string category)
        {
            SelectedCategoryStateContainer.SelectedCategory = category;
            SelectedCategory = category;
            ToDos = ToDoStateContainer.TryGetToDos(category, out var toDos)
                ? toDos
                    .Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))
                    .ToList()
                : new();

            IsDropdown = false;
            await CategoryListView.HideAsync();
            await ((IJSInProcessRuntime)Js).InvokeVoidAsync("setScroll", true);
        }

        protected void RefreshData()
        {
            SelectedCategory = GetCategory();
            ToDos = ToDoStateContainer.TryGetToDos(SelectedCategory, out var toDos)
                ? toDos
                    .Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))
                    .ToList()
                : new();

            string GetCategory()
            {
                var existingCategory = SelectedCategoryStateContainer.SelectedCategory;
                var hasToDos = ToDoStateContainer.TryGetToDos(existingCategory, out _);
                if (!hasToDos)
                {
                    var newCategory = ToDoStateContainer.Categories.Any() ? ToDoStateContainer.Categories[0] : "";
                    SelectedCategoryStateContainer.SelectedCategory = newCategory;
                    return newCategory;
                }
                return existingCategory;
            }
        }
    }
}
