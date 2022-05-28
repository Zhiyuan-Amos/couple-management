namespace Couple.Shared.Models.Issue;

public class TaskDto
{
    public TaskDto(Guid id, string content)
    {
        Id = id;
        Content = content;
    }

    public Guid Id { get; }
    public string Content { get; }
}
