namespace Couple.Client.ViewModel.Issue
{
    public interface IReadOnlyTaskViewModel
    {
        string Content { get; }
        bool IsCompleted { get; }
    }
}
