using System.Text.Json.Serialization;

namespace Couple.Shared.Models.Issue;

public class CreateIssueDto
{
    public CreateIssueDto(Guid id, string title, For @for, IEnumerable<TaskDto> tasks, DateTime createdOn)
        : this(id, title, @for, new(tasks), createdOn) { }
    
    [JsonConstructor]
    public CreateIssueDto(Guid id, string title, For @for, List<TaskDto> tasks, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Tasks = tasks;
        CreatedOn = createdOn;
    }

    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public List<TaskDto> Tasks { get; }
    public DateTime CreatedOn { get; }
}
