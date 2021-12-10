using System;
using System.Collections.Generic;
using Couple.Shared.Model;

namespace Couple.Client.Model.Issue;

public class IssueModel
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public For For { get; init; }
    public IReadOnlyList<TaskModel> Tasks { get; init; }
    public DateTime CreatedOn { get; init; }

    public IssueModel() { }

    public IssueModel(Guid id, string title, For @for, IEnumerable<TaskModel> tasks, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Tasks = new List<TaskModel>(tasks);
        CreatedOn = createdOn;
    }
}
