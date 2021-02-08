using System;

namespace Couple.Client.ViewModel.ToDo
{
    public class ToDoViewModel
    {
        public Guid Id { get; }
        public string Text { get; }
        public string Category { get; }
        public DateTime CreatedOn { get; }

        public ToDoViewModel(Guid id, string text, string category, DateTime createdOn)
            => (Id, Text, Category, CreatedOn) = (id, text, category, createdOn);
    }
}
