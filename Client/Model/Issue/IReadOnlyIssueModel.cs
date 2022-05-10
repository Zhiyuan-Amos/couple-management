using Couple.Shared.Model;

namespace Couple.Client.Model.Issue;

public interface IReadOnlyIssueModel
{
    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public IReadOnlyList<IReadOnlyTaskModel> ReadOnlyTasks { get; }
    public DateTime CreatedOn { get; }
}
