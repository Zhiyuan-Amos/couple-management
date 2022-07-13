namespace Couple.Client.Features.Issue.ViewModels;

public class CreateUpdateTaskViewModel : IReadOnlyTaskViewModel
{
    public CreateUpdateTaskViewModel(string content)
    {
        Id = Guid.NewGuid();
        Content = content;
    }

    public CreateUpdateTaskViewModel(Guid id, string content)
    {
        Id = id;
        Content = content;
    }

    public Guid Id { get; }
    public string Content { get; set; }
}
