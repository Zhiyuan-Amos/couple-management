using Couple.Client.ViewModel.ToDo;
using System;
using System.Collections.Generic;

namespace Couple.Client.Model.Event
{
    public class CreateEventModel
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<ToDoViewModel> ToDos { get; set; }
    }
}
