using Couple.Shared.Model;

namespace Couple.Client.Model.Done;

public class DoneIssueModel : IDone, IReadOnlyDoneIssueModel
{
    public DoneIssueModel(DateOnly doneDate, IEnumerable<DoneTaskModel> doneTasks, For @for, string title) =>
        (DoneDate, Tasks, For, Title) = (doneDate, new(doneTasks), @for, title);

#pragma warning disable IDE0051
    // ReSharper disable once UnusedMember.Local
    private DoneIssueModel(Guid id, DateOnly doneDate, int order, List<DoneTaskModel> tasks, For @for, string title) =>
        (Id, DoneDate, Order, Tasks, For, Title) = (id, doneDate, order, tasks, @for, title);
#pragma warning restore IDE0051

    public List<DoneTaskModel> Tasks { get; }

    public int Order { get; set; }

    public DateOnly DoneDate { get; init; }

    public string DoneDatePropertyName => "DoneDate";

    public Guid Id { get; }
    public string Title { get; init; }
    public For For { get; init; }

    public IReadOnlyList<IReadOnlyDoneTaskModel> ReadOnlyTasks => Tasks.AsReadOnly();
}
