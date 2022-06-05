namespace Couple.Shared.Models.Calendar;

public class UpdateEventDto
{
    public UpdateEventDto(Guid id, string title, For @for, DateTime start, DateTime end, DateTime createdOn)
    {
        Id = id;
        Title = title;
        For = @for;
        Start = start;
        End = end;
        CreatedOn = createdOn;
    }

    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public DateTime Start { get; }
    public DateTime End { get; }
    public DateTime CreatedOn { get; }
}
