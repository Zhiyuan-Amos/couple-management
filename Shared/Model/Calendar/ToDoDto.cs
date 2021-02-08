using System;

namespace Couple.Shared.Model.Calendar
{
    public class ToDoDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}