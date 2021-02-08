using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Components.Calendar
{
    public partial class CategoryTreeViewContent
    {
        [Parameter]
        public List<ToDoViewModel> Added { get; set; }

        [Parameter]
        public List<ToDoViewModel> Removed { get; set; }

        [Parameter]
        public List<ToDoViewModel> Selected { get; set; }

        [Parameter]
        public EventCallback<List<ToDoViewModel>> SelectedChanged { get; set; }

        private ToDoDataState ToDoDataState => GetState<ToDoDataState>();

        protected List<CategoryToDos> Data { get; set; }

        protected override void OnInitialized()
        {
            var categoryToToDos = ToDoDataState
                .GetToDos()
                .Where(toDo => Added.All(add => add.Id != toDo.Id))
                .Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))
                .Concat(Removed)
                .GroupBy(toDo => toDo.Category)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());

            Data = categoryToToDos
                .Select(kvp => new CategoryToDos(kvp.Key, kvp.Value))
                .ToList();
        }

        public async Task SelectedObjectsChanged(IEnumerable<object> objs) =>
            await SelectedChanged.InvokeAsync(objs.Select(obj => obj as ToDoViewModel).ToList());

        protected class CategoryToDos
        {
            public string Category { get; }
            public List<ToDoViewModel> ToDos { get; }
            public CategoryToDos(string category, List<ToDoViewModel> toDos) => (Category, ToDos) = (category, toDos);
        }
    }
}
