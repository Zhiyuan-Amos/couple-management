namespace Couple.Shared.Model.Event;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public List<IssueDto> ToDos { get; set; }
}
