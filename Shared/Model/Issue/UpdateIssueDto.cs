using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Issue
{
    public class UpdateIssueDto
    {
        public Guid Id { get; }
        public string Title { get; }
        public For For { get; }
        public List<TaskDto> Tasks { get; }
        public DateTime CreatedOn { get; }

        public UpdateIssueDto(Guid id, string title, For @for, List<TaskDto> tasks, DateTime createdOn)
        {
            Id = id;
            Title = title;
            For = @for;
            Tasks = new(tasks);
            CreatedOn = createdOn;
        }
    }
}
