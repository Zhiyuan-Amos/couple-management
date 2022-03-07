using Couple.Shared.Model;

namespace Couple.Client.Model.Issue;

public class IssueModel : IReadOnlyIssueModel
{
    private IssueModel() { }

    public IssueModel(Guid id) => Id = id;

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

    public List<TaskModel> Tasks { get; set; }

    public Guid Id { get; }
    public string Title { get; set; }
    public For For { get; set; }
    public DateTime CreatedOn { get; init; }

    public IReadOnlyList<IReadOnlyTaskModel> ReadOnlyTasks => Tasks.AsReadOnly();
}
