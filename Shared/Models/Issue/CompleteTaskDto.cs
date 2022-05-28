using System.Text.Json.Serialization;
using Couple.Shared.Converters;

namespace Couple.Shared.Models.Issue;

public class CompleteTaskDto
{
    public CompleteTaskDto(Guid taskId, Guid issueId, DateOnly completedDate)
    {
        TaskId = taskId;
        IssueId = issueId;
        CompletedDate = completedDate;
    }

    public Guid TaskId { get; }
    public Guid IssueId { get; }

    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly CompletedDate { get; }
}
