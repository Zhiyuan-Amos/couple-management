using Couple.Shared.Model;

namespace Couple.Client.ViewModel.Issue;

public class CompletedTaskViewModel
{
    public CompletedTaskViewModel(For @for,
        List<string> contents,
        string issueTitle,
        DateTime createdOn) =>
        (For, Contents, IssueTitle, CreatedOn) = (@for, new(contents), issueTitle, createdOn);

    public For For { get; }
    public List<string> Contents { get; }

    public string IssueTitle { get; }
    public DateTime CreatedOn { get; }
}
