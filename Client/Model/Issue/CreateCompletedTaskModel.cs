namespace Couple.Client.Model.Issue;

public class CreateCompletedTaskModel
{
    public CreateCompletedTaskModel(Guid taskId, Guid issueId, DateOnly completedDate)
    {
        TaskId = taskId;
        IssueId = issueId;
        CompletedDate = completedDate;
    }

    public Guid TaskId { get; }
    public Guid IssueId { get; }
    public DateOnly CompletedDate { get; }
}
