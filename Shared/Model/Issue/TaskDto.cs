using System;

namespace Couple.Shared.Model.Issue
{
    public class TaskDto
    {
        public Guid Id { get; }
        public string Content { get; }

        public TaskDto(Guid id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
