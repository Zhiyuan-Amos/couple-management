namespace Couple.Client.Features.Done.Models;

public interface IReadOnlyDoneTaskModel
{
    public Guid Id { get; }
    public string Content { get; }
}
