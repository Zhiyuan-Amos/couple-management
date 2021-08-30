namespace Couple.Client.ViewModel.Issue
{
    public class CreateUpdateTaskViewModel : IReadOnlyTaskViewModel
    {
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
    }
}
