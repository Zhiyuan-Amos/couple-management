using System;

namespace Couple.Shared.Model.Issue
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
    }
}
