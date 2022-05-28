namespace Couple.Client.Features.Issue.Models;

public interface IReadOnlyTaskModel
{
    public Guid Id { get; }
    public string Content { get; }
}
