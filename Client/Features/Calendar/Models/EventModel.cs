using Couple.Shared.Models;

namespace Couple.Client.Features.Calendar.Models;

public class EventModel : IReadOnlyEventModel
{
    public EventModel(Guid id, string title, For @for, DateTime start, DateTime end, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Start = start;
        End = end;
        CreatedOn = createdOn;
    }
    
    public EventModel(string title, For @for, DateTime start, DateTime end, DateTime createdOn)
    {
        Title = title;
        For = @for;
        Start = start;
        End = end;
        CreatedOn = createdOn;
    }

    public Guid Id { get; }
    public string Title { get; set; }
    public For For { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime CreatedOn { get; init; }
}
