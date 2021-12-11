using Couple.Client.Model.Calendar;

namespace Couple.Client.States.Calendar;

public class EventStateContainer : Notifier
{
    private Dictionary<DateTime, List<EventModel>> DateToEvents { get; set; } = new();

    public bool TryGetEvents(DateTime dateTime, out List<EventModel> events)
    {
        return DateToEvents.TryGetValue(dateTime, out events);
    }

    public bool TryGetEvent(Guid id, out EventModel @event)
    {
        @event = DateToEvents.Values
            .SelectMany(events => events)
            .ToList()
            .FirstOrDefault(innerEvent => innerEvent.Id == id);

        return @event != null;
    }

    public void SetEvents(IEnumerable<EventModel> events)
    {
        DateToEvents = events
            .GroupBy(@event => @event.Start.Date)
            .ToDictionary(grouping => grouping.Key, grouping => grouping
                .OrderBy(@event => @event.Start)
                .ToList());

        NotifyStateChanged();
    }
}
