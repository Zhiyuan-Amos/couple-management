using Couple.Shared.Model;

namespace Couple.Client.Model.Issue;

public class IssueModel
{
    private IssueModel() { }

    public IssueModel(Guid id) => Id = id;

    public IssueModel(string title, For @for, IEnumerable<TaskModel> tasks, DateTime createdOn)
    {
        Title = title;
        For = @for;
        Tasks = new List<TaskModel>(tasks);
        CreatedOn = createdOn;
    }

    public IssueModel(Guid id, string title, For @for, IEnumerable<TaskModel> tasks, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Tasks = new List<TaskModel>(tasks);
        CreatedOn = createdOn;
    }

    public Guid Id { get; }
    public string Title { get; set; }
    public For For { get; set; }
    public IReadOnlyList<TaskModel> Tasks { get; set; }
    public DateTime CreatedOn { get; init; }
}
