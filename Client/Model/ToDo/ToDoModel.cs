using Couple.Shared.Model;
using System;
using System.Collections.Generic;

namespace Couple.Client.Model.ToDo
{
    public class ToDoModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public For For { get; init; }
        public List<ToDoInnerModel> ToDos { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}
