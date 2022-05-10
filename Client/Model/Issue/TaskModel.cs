using System.Text.Json.Serialization;

namespace Couple.Client.Model.Issue;

public class TaskModel : IReadOnlyTaskModel
{
    public TaskModel(string content)
    {
        Id = Guid.NewGuid();
        Content = content;
    }

    [JsonConstructor]
    public TaskModel(Guid id, string content) =>
        (Id, Content) = (id, content);

    public Guid Id { get; }
    public string Content { get; init; }

    private bool Equals(TaskModel other) => Id.Equals(other.Id) && Content == other.Content;

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((TaskModel)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Id, Content);
}
