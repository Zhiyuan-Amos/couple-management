using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class SelectedCategoryStateContainer
    {
        private readonly ToDoStateContainer _toDoStateContainer;

        public string SelectedCategory { get; set; }

        public SelectedCategoryStateContainer(ToDoStateContainer toDoStateContainer)
        {
            _toDoStateContainer = toDoStateContainer;
        }

        public void Reset() => SelectedCategory = _toDoStateContainer.Categories.Any() ? _toDoStateContainer.Categories[0] : "";
    }
}
