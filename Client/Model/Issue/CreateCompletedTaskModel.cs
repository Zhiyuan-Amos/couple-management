using Couple.Shared.Model;

namespace Couple.Client.Model.Issue;

public class CreateCompletedTaskModel
{
    public CreateCompletedTaskModel(Guid id, For @for, string content, Guid issueId, string issueTitle,
        DateTime createdOn)
    {
        Id = id;
        For = @for;
        Content = content;
        IssueId = issueId;
        IssueTitle = issueTitle;
        CreatedOn = createdOn;
    }

    public Guid Id { get; }
    public For For { get; }
    public string Content { get; }

    public Guid IssueId { get; }
    public string IssueTitle { get; }
    public DateTime CreatedOn { get; }
}
