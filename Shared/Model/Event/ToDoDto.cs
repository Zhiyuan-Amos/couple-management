using Couple.Shared.Model.ToDo;
using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Event
{
    public class ToDoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public For For { get; set; }
        public List<ToDoInnerDto> ToDos { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
