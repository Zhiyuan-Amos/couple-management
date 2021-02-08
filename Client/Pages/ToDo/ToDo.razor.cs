using Couple.Client.Components.ToDo;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Couple.Client.States.ToDo.SelectedCategoryState;

namespace Couple.Client.Pages.ToDo
{
    public partial class ToDo
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        private IJSRuntime Js { get; set; }

        private AnimatedCategoryListView CategoryListView { get; set; }

        private ToDoDataState ToDoDataState => GetState<ToDoDataState>();

        protected bool IsDropdown { get; set; }
        protected string SelectedCategory { get; set; }
        protected List<ToDoViewModel> ToDos { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshDataAsync();
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
            await Mediator.Send(new ModifySelectedCategoryAction(category));
            SelectedCategory = category;
            ToDos = ToDoDataState.TryGetToDos(category, out var toDos)
                ? toDos
                    .Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))
                    .ToList()
                : new();

            IsDropdown = false;
            await CategoryListView.HideAsync();
            await ((IJSInProcessRuntime)Js).InvokeVoidAsync("setScroll", true);
        }

        protected async Task RefreshDataAsync()
        {
            SelectedCategory = await GetCategory();
            ToDos = ToDoDataState.TryGetToDos(SelectedCategory, out var toDos)
                ? toDos
                    .Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))
                    .ToList()
                : new();

            async Task<string> GetCategory()
            {
                var existingCategory = GetState<SelectedCategoryState>().SelectedCategory;
                var hasToDos = ToDoDataState.TryGetToDos(existingCategory, out _);
                if (!hasToDos)
                {
                    var newCategory = ToDoDataState.Categories.Any() ? ToDoDataState.Categories[0] : "";
                    await Mediator.Send(new ModifySelectedCategoryAction(newCategory));
                    return newCategory;
                }
                return existingCategory;
            }
        }
    }
}
