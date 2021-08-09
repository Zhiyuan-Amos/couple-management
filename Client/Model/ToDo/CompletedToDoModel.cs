using System;

namespace Couple.Client.Model.ToDo
{
    public class CompletedToDoModel : ToDoModel
    {
        public DateTime CompletedOn { get; init; }
    }
}