namespace Couple.Client.Model.Issue;

public class TaskModel
{
    public TaskModel(Guid id, string content)
    {
        Id = id;
        Content = content;
    }

    public Guid Id { get; }
    public string Content { get; }
}
