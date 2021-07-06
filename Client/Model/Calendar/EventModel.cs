using Couple.Client.Model.ToDo;
using System;
using System.Collections.Generic;

namespace Couple.Client.Model.Calendar
{
    public class EventModel
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public IReadOnlyList<ToDoModel> ToDos { get; init; }
    }
}
