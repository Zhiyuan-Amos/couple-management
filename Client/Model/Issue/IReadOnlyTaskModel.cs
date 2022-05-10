namespace Couple.Client.Model.Issue;

public interface IReadOnlyTaskModel
{
    public Guid Id { get; }
    public string Content { get; }
}
