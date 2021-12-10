using Couple.Client.Model.Issue;

namespace Couple.Client.ViewModel.Calendar;

public class UpdateEventViewModel
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public List<IssueModel> ToDos { get; set; }
}
