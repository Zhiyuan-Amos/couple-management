using Couple.Shared.Model;

namespace Couple.Client.Model.Done;

public class DoneIssueModel : IDone
{
    private DoneIssueModel() { }

    public DoneIssueModel(DateOnly doneDate, List<DoneTaskModel> doneTasks, For @for, string issueTitle) =>
        (DoneDate, Tasks, For, Title) = (doneDate, doneTasks, @for, issueTitle);

    public Guid Id { get; }
    public string Title { get; init; }
    public For For { get; init; }
    public List<DoneTaskModel> Tasks { get; }

    public int Order { get; set; }

    public DateOnly DoneDate { get; init; }
}
