using Couple.Shared.Models;

namespace Couple.Client.Features.Done.Models;

public interface IReadOnlyDoneIssueModel
{
    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public IReadOnlyList<IReadOnlyDoneTaskModel> ReadOnlyTasks { get; }

    public int Order { get; }

    public DateOnly DoneDate { get; }
}
