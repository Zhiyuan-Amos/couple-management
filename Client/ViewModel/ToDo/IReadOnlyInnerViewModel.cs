namespace Couple.Client.ViewModel.ToDo
{
    public interface IReadOnlyInnerViewModel
    {
        string Content { get; }
        bool IsCompleted { get; }
    }
}
