using System;
using System.Collections.Generic;
using Couple.Shared.Model;

namespace Couple.Client.Model.Issue
{
    public class IssueModel
    {
        public Guid Id { get; }
        public string Title { get; }
        public For For { get; }
        public IReadOnlyList<TaskModel> Tasks { get; }
        public DateTime CreatedOn { get; }

        public IssueModel(Guid id, string title, For @for, List<TaskModel> tasks, DateTime createdOn)
        {
            Id = id;
            Title = title;
            For = @for;
            Tasks = tasks;
            CreatedOn = createdOn;
        }
    }
}
