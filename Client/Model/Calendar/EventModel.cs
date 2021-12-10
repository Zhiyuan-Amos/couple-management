using Couple.Client.Model.Issue;

namespace Couple.Client.Model.Calendar;

public class EventModel
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public IReadOnlyList<IssueModel> ToDos { get; init; }
}
