using Couple.Shared.Model;
using System;
using System.Collections.Generic;

namespace Couple.Client.ViewModel.ToDo
{
    public class CompletedToDoViewModel
    {
        public string Name { get; }
        public For For { get; }
        public List<string> ToDos { get; }
        public DateTime CompletedOn { get; }

        public CompletedToDoViewModel(string name, For @for, List<string> toDos, DateTime completedOn)
            => (Name, For, ToDos, CompletedOn) = (name, @for, toDos, completedOn);
    }
}
