﻿using Couple.Shared.Model;

namespace Couple.Client.Model.Done;

public class DoneIssueModel : IDone, IReadOnlyDoneIssueModel
{
    private DoneIssueModel() { }

    public DoneIssueModel(DateOnly doneDate, IEnumerable<DoneTaskModel> doneTasks, For @for, string issueTitle) =>
        (DoneDate, Tasks, For, Title) = (doneDate, new(doneTasks), @for, issueTitle);

    public List<DoneTaskModel> Tasks { get; }

    public int Order { get; set; }

    public DateOnly DoneDate { get; init; }

    public Guid Id { get; }
    public string Title { get; init; }
    public For For { get; init; }

    public IReadOnlyList<IReadOnlyDoneTaskModel> ReadOnlyTasks => Tasks.AsReadOnly();
}
