using Couple.Shared.Models;

namespace Couple.Client.Features.Issue.Models;

public class IssueModel : IReadOnlyIssueModel
{
    public IssueModel(string title, For @for, IEnumerable<TaskModel> tasks, DateTime createdOn)
    {
        Title = title;
        For = @for;
        Tasks = new(tasks);
        CreatedOn = createdOn;
    }

    public IssueModel(Guid id, string title, For @for, IEnumerable<IReadOnlyTaskModel> tasks, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Tasks = tasks.Select(t => new TaskModel(t.Id, t.Content)).ToList();
        CreatedOn = createdOn;
    }

#pragma warning disable IDE0051
    // ReSharper disable once UnusedMember.Local
    private IssueModel(Guid id, string title, For @for, List<TaskModel> tasks, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Tasks = tasks;
        CreatedOn = createdOn;
    }
#pragma warning restore IDE0051

    public List<TaskModel> Tasks { get; set; }

    public Guid Id { get; }
    public string Title { get; set; }
    public For For { get; set; }
    public DateTime CreatedOn { get; init; }

    public IReadOnlyList<IReadOnlyTaskModel> ReadOnlyTasks => Tasks.AsReadOnly();
}
