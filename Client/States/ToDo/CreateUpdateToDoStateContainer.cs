using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class CreateUpdateToDoStateContainer
    {
        public string Name { get; set; }
        public For For { get; set; }
        private List<CreateUpdateInnerViewModel> _toDos;

        public IReadOnlyList<IReadOnlyInnerViewModel> ToDos => _toDos;

        public void AddToDo(string content, bool isCompleted)
        {
            _toDos.Add(new() {Content = content, IsCompleted = isCompleted,});
        }

        public void TrimToDos() => _toDos.RemoveAll(toDo => !toDo.Content.Any());

        public void SetContent(int index, string content)
        {
            _toDos[index].Content = content;
        }

        public void Initialize(string name, For @for, IEnumerable<ToDoInnerModel> toDos)
        {
            Name = name;
            For = @for;
            _toDos = ToDoAdapter.ToCreateUpdateInnerViewModel(toDos);
        }

        public void Reset()
        {
            Name = null;
            For = For.Him;
            _toDos = new();
        }
    }
}
