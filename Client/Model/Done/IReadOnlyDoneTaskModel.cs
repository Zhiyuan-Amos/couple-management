namespace Couple.Client.Model.Done;

public interface IReadOnlyDoneTaskModel
{
    public Guid Id { get; }
    public string Content { get; }
}
