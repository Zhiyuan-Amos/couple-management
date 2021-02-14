using Couple.Client.Infrastructure;
using Couple.Client.States.Calendar;
using Couple.Client.ViewModel.Calendar;
using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.Pages.Calendar
{
    public partial class Calendar
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        private EventStateContainer EventStateContainer { get; set; }

        [Parameter]
        public DateTime Selected { get; set; }

        protected List<EventViewModel> Events { get; set; }

        protected override void OnInitialized()
        {
            if (Selected == new DateTime())
            {
                Selected = DateTime.Now.Date;
                NavigationManager.NavigateTo($"/calendar/{Selected.ToCalendarUrl()}");
            }

            Events = GetEvents(Selected);
        }

        protected void DateChangedHandler(DateTime newDateValue)
        {
            NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");
            Events = GetEvents(newDateValue);
        }

        protected void ValueChangedHandler(DateTime newDateValue)
        {
            NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");
            Events = GetEvents(newDateValue);
        }

        protected void AddEvent() => NavigationManager.NavigateTo($"/calendar/create");
        protected void EditEvent(EventViewModel selectedEvent) => NavigationManager.NavigateTo($"/calendar/{selectedEvent.Id}");

        private List<EventViewModel> GetEvents(DateTime dateTime)
        {
            return EventStateContainer.TryGetEvents(dateTime, out var events)
                ? events
                    .Select(@event => new EventViewModel(
                        @event.Id,
                        @event.Title,
                        @event.Start,
                        @event.End,
                        @event.ToDos
                            .Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))
                            .ToList()))
                    .ToList()
                : new();
        }

        protected void RefreshEvents() => Events = GetEvents(Selected);
    }
}
