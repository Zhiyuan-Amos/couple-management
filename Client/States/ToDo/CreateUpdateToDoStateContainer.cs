using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class CreateUpdateToDoStateContainer : Notifier
    {
        public Guid Id { get; private set; }
        private string _name;
        private For _for;
        private List<CreateUpdateInnerViewModel> _toDos;
        public DateTime CreatedOn { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyStateChanged();
            }
        }

        public For For
        {
            get => _for;
            set
            {
                _for = value;
                NotifyStateChanged();
            }
        }

        public IReadOnlyList<IReadOnlyInnerViewModel> ToDos => _toDos;

        public void AddToDo(string content, bool isCompleted)
        {
            _toDos.Add(new() {Content = content, IsCompleted = isCompleted,});
            NotifyStateChanged();
        }

        public void TrimToDos() => _toDos.RemoveAll(toDo => !toDo.Content.Any());

        public void SetContent(int index, string content)
        {
            _toDos[index].Content = content;
            NotifyStateChanged();
        }

        public void Initialize(string name, For @for, IEnumerable<ToDoInnerModel> toDos)
        {
            _name = name;
            _for = @for;
            _toDos = ToDoAdapter.ToCreateUpdateInnerViewModel(toDos);
        }

        public void Initialize(Guid id, string name, For @for, IEnumerable<ToDoInnerModel> toDos, DateTime createdOn)
        {
            Id = id;
            _name = name;
            _for = @for;
            _toDos = ToDoAdapter.ToCreateUpdateInnerViewModel(toDos);
            CreatedOn = createdOn;
        }

        public void Reset()
        {
            Id = Guid.Empty;
            _name = null;
            _for = For.Him;
            _toDos = new();
            CreatedOn = DateTime.UnixEpoch;
        }
    }
}
