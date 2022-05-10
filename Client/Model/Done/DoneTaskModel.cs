using System.Text.Json.Serialization;

namespace Couple.Client.Model.Done;

public class DoneTaskModel : IReadOnlyDoneTaskModel
{
    public DoneTaskModel(string content)
    {
        Id = Guid.NewGuid();
        Content = content;
    }

#pragma warning disable IDE0051
    [JsonConstructor]
    private DoneTaskModel(Guid id, string content) =>
        (Id, Content) = (id, content);
#pragma warning restore IDE0051

    public Guid Id { get; }
    public string Content { get; init; }

    protected bool Equals(DoneTaskModel other) => Id.Equals(other.Id) && Content == other.Content;

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

        return Equals((DoneTaskModel)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Id, Content);
}
