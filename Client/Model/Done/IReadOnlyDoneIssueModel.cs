using Couple.Shared.Model;

namespace Couple.Client.Model.Done;

public interface IReadOnlyDoneIssueModel
{
    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public IReadOnlyList<IReadOnlyDoneTaskModel> ReadOnlyTasks { get; }

    public int Order { get; }

    public DateOnly DoneDate { get; }
}