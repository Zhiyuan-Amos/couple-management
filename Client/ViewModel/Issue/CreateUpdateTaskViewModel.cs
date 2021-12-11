namespace Couple.Client.ViewModel.Issue;

public class CreateUpdateTaskViewModel : IReadOnlyTaskViewModel
{
    public CreateUpdateTaskViewModel(Guid id, string content)
    {
        Id = id;
        Content = content;
    }

    public Guid Id { get; }
    public string Content { get; set; }
}
