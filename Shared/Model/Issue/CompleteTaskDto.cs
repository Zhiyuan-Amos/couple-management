namespace Couple.Shared.Model.Issue;

public class CompleteTaskDto
{
    public CompleteTaskDto(Guid id, For @for, string content, Guid issueId, string issueTitle, DateTime createdOn)
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
