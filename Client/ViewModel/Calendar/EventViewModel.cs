using Couple.Client.Model.Issue;
using System;
using System.Collections.Generic;

namespace Couple.Client.ViewModel.Calendar;

public class EventViewModel
{
    public Guid Id { get; }
    public string Title { get; }
    public DateTime Start { get; }
    public DateTime End { get; }
    public List<IssueModel> ToDos { get; }

    public EventViewModel(Guid id, string title, DateTime start, DateTime end, List<IssueModel> toDos)
        => (Id, Title, Start, End, ToDos) = (id, title, start, end, toDos);
}
