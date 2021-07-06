using Couple.Client.Model.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.ToDo
{
    public class ToDoStateContainer : Notifier
    {
        private List<ToDoModel> _toDos = new();
        private Dictionary<Guid, ToDoModel> _idToToDo = new();

        public List<ToDoModel> ToDos
        {
            get => _toDos;
            set
            {
                _toDos = value.ToList();
                _idToToDo = value.ToDictionary(toDo => toDo.Id);
                NotifyStateChanged();
            }
        }

        public bool TryGetToDo(Guid id, out ToDoModel toDo)
        {
            if (!_idToToDo.TryGetValue(id, out toDo))
            {
                return false;
            }

            return true;
        }
    }
}
