using Couple.Client.Model.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.Calendar
{
    public class EventStateContainer
    {
        private Dictionary<DateTime, List<EventModel>> DateToEvents { get; set; }

        public bool TryGetEvents(DateTime dateTime, out List<EventModel> events)
        {
            if (!DateToEvents.TryGetValue(dateTime, out events))
            {
                return false;
            }

            return true;
        }

        public bool TryGetEvent(Guid id, out EventModel @event)
        {
            @event = DateToEvents.Values
                .SelectMany(events => events)
                .ToList()
                .FirstOrDefault(@event => @event.Id == id);

            if (@event == null)
            {
                return false;
            }

            return true;
        }

        public void SetEvents(List<EventModel> events)
        {
            DateToEvents = events
                .GroupBy(@event => @event.Start.Date)
                .ToDictionary(grouping => grouping.Key, grouping => grouping
                    .OrderBy(@event => @event.Start)
                    .ToList());
        }
    }
}
