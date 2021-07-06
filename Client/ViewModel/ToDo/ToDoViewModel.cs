using Couple.Shared.Model;
using System;
using System.Collections.Generic;

namespace Couple.Client.ViewModel.ToDo
{
    public class ToDoViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public For For { get; }
        public List<ToDoInnerViewModel> ToDos { get; }
        public DateTime CreatedOn { get; }

        public ToDoViewModel(Guid id, string name, For @for, List<ToDoInnerViewModel> toDos, DateTime createdOn)
            => (Id, Name, For, ToDos, CreatedOn) = (id, name, @for, toDos, createdOn);
    }
}
