namespace Couple.Client.Features.Issue.ViewModels;

public interface IReadOnlyTaskViewModel
{
    Guid Id { get; }
    string Content { get; }
}
