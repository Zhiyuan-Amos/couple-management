using System;

namespace Couple.Client.Model.Issue;

public class TaskModel
{
    public Guid Id { get; }
    public string Content { get; }

    public TaskModel(Guid id, string content)
    {
        Id = id;
        Content = content;
    }
}
