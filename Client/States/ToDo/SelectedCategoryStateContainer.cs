using Couple.Client.Data;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class SelectedCategoryStateContainer
    {
        private readonly LocalStore _localStore;
        private readonly ToDoStateContainer _toDoStateContainer;

        public string SelectedCategory { get; set; }

        public SelectedCategoryStateContainer(LocalStore localStore, ToDoStateContainer toDoStateContainer)
        {
            _localStore = localStore;
            _toDoStateContainer = toDoStateContainer;
        }

        public void Reset() => SelectedCategory = _toDoStateContainer.Categories.Any() ? _toDoStateContainer.Categories[0] : "";
    }
}
