using System.Text.Json.Serialization;

namespace Couple.Shared.Models.Issue;

public class UpdateIssueDto
{
    public UpdateIssueDto(Guid id, string title, For @for, IEnumerable<TaskDto> tasks)
        : this(id, title, @for, new(tasks)) { }

    [JsonConstructor]
    public UpdateIssueDto(Guid id, string title, For @for, List<TaskDto> tasks)
    {
        Id = id;
        Title = title;
        For = @for;
        Tasks = tasks;
    }

    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public List<TaskDto> Tasks { get; }
}
