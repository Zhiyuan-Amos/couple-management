using System;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class SelectedCategoryStateContainer : IDisposable
    {
        private readonly ToDoStateContainer _toDoStateContainer;

        public string SelectedCategory { get; set; }

        public SelectedCategoryStateContainer(ToDoStateContainer toDoStateContainer)
        {
            _toDoStateContainer = toDoStateContainer;
            _toDoStateContainer.OnChange += Refresh;
        }

        public void Reset() => SelectedCategory = _toDoStateContainer.Categories.Any() ? _toDoStateContainer.Categories[0] : "";

        private void Refresh()
        {
            var hasToDos = _toDoStateContainer.TryGetToDos(SelectedCategory, out _);

            if (hasToDos)
            {
                return;
            }

            Reset();
        }

        public void Dispose() => _toDoStateContainer.OnChange -= Refresh;
    }
}
