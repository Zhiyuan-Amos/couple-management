using AutoMapper;
using AutoMapper.QueryableExtensions;
using Couple.Client.Pages.ToDo.Components;
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
        private NavigationManager NavigationManager { get; init; }

        [Inject]
        private IJSRuntime Js { get; init; }

        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; init; }

        [Inject]
        private SelectedCategoryStateContainer SelectedCategoryStateContainer { get; init; }

        [Inject]
        private IMapper Mapper { get; init; }

        private AnimatedCategoryListView CategoryListView { get; set; }

        protected bool IsDropdown { get; set; }
        protected string SelectedCategory => SelectedCategoryStateContainer.SelectedCategory;

        protected List<ToDoViewModel> ToDos =>
            ToDoStateContainer.TryGetToDos(SelectedCategory, out var toDos)
                ? toDos
                    .AsQueryable()
                    .ProjectTo<ToDoViewModel>(Mapper.ConfigurationProvider)
                    .ToList()
                : new();

        protected override void OnInitialized()
        {
            ToDoStateContainer.OnChange += StateHasChanged;
        }

        public void Dispose() => ToDoStateContainer.OnChange -= StateHasChanged;

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

            IsDropdown = false;
            await CategoryListView.HideAsync();
            await ((IJSInProcessRuntime)Js).InvokeVoidAsync("setScroll", true);
        }
    }
}
