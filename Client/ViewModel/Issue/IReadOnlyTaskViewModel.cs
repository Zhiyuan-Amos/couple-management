namespace Couple.Client.ViewModel.Issue;

public interface IReadOnlyTaskViewModel
{
    Guid Id { get; }
    string Content { get; }
}
