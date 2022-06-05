using Couple.Shared.Models;

namespace Couple.Client.Features.Calendar.States;

public class CreateUpdateEventStateContainer
{
    public CreateUpdateEventStateContainer(string title, For @for, DateTime start, DateTime end)
    {
        Title = title;
        For = @for;
        End = end; // Order of setting matters because of logic in setter; DateTime defaults to 00/00/0001
        Start = start;
    }

    public string Title { get; set; }
    public For For { get; set; }

    private DateTime _start;
    private DateTime _end;

    public DateTime Start
    {
        get => _start;
        set
        {
            if (value > End) return;
            _start = value;
        }
    }

    public DateTime End
    {
        get => _end;
        set
        {
            if (value < Start) return;
            _end = value;
        } 
    }
}
