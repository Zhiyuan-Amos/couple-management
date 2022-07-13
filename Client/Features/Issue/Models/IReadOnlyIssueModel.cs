using Couple.Shared.Models;

namespace Couple.Client.Features.Issue.Models;

public interface IReadOnlyIssueModel
{
    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public IReadOnlyList<IReadOnlyTaskModel> ReadOnlyTasks { get; }
    public DateTime CreatedOn { get; }
}
