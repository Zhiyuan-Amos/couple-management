using BlazorState;
using Couple.Client.Data;
using Couple.Client.Data.Calendar;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Couple.Client.States.Calendar
{
    public partial class EventDataState : State<EventDataState>
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

        public override void Initialize() { }

        public class RefreshEventsAction : IAction
        {
            public LocalStore LocalStore { get; }

            public RefreshEventsAction(LocalStore localStore) => (LocalStore) = (localStore);
        }

        public class RefreshEventsHandler : ActionHandler<RefreshEventsAction>
        {
            private EventDataState EventDataState => Store.GetState<EventDataState>();

            public RefreshEventsHandler(IStore store) : base(store) { }

            public override async Task<Unit> Handle(RefreshEventsAction refreshEventsAction, CancellationToken cancellationToken)
            {
                var events = await refreshEventsAction.LocalStore.GetAllAsync<List<EventModel>>("event");
                EventDataState.DateToEvents = events
                    .GroupBy(@event => @event.Start.Date)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping
                        .OrderBy(@event => @event.Start)
                        .ToList());

                return Unit.Value;
            }
        }
    }
}
