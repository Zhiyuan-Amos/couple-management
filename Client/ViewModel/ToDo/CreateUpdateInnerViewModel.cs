namespace Couple.Client.ViewModel.ToDo
{
    public class CreateUpdateInnerViewModel : IReadOnlyInnerViewModel
    {
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
    }
}
