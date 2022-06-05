using Couple.Client.Features.Calendar.Models;
using Couple.Client.Shared.States;

namespace Couple.Client.Features.Calendar.States;

public class EventStateContainer : Notifier
{
    private readonly List<IReadOnlyEventModel> _events = new();
    private Dictionary<Guid, IReadOnlyEventModel> _idToEvent = new();

    public IReadOnlyList<IReadOnlyEventModel> Events
    {
        get => _events.AsReadOnly();
        set
        {
            _events.Clear();
            _events.AddRange(value);

            _idToEvent = value.ToDictionary(issue => issue.Id);

            NotifyStateChanged();
        }
    }

    public void AddEvent(EventModel @event)
    {
        _events.Add(@event);
        _idToEvent.Add(@event.Id, @event);

        NotifyStateChanged();
    }

    public void UpdateEvent(EventModel @event)
    {
        _idToEvent.Remove(@event.Id, out var oldEvent);
        _events.Remove(oldEvent!);

        _events.Add(@event);
        _idToEvent.Add(@event.Id, @event);

        NotifyStateChanged();
    }

    public void DeleteEvent(Guid eventId)
    {
        _idToEvent.Remove(eventId, out var oldEvent);
        _events.Remove(oldEvent!);

        NotifyStateChanged();
    }

    public bool TryGetEvent(Guid id, out IReadOnlyEventModel? readOnlyEvent) =>
        _idToEvent.TryGetValue(id, out readOnlyEvent);
}
