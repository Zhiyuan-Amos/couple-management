using AutoMapper;
using Couple.Client.Infrastructure;
using Couple.Client.States.Calendar;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Couple.Client.Pages.Calendar
{
    public partial class Calendar
    {
        [Inject]
        private NavigationManager NavigationManager { get; init; }

        [Inject]
        private EventStateContainer EventStateContainer { get; init; }

        [Inject]
        private IMapper Mapper { get; init; }

        [Parameter]
        public DateTime Selected { get; set; }

        protected IEnumerable<EventViewModel> Events => EventStateContainer.TryGetEvents(Selected, out var events)
            ? Mapper.Map<List<EventViewModel>>(events)
            : new();

        protected override void OnInitialized()
        {
            if (Selected == new DateTime())
            {
                Selected = DateTime.Now.Date;
                NavigationManager.NavigateTo($"/calendar/{Selected.ToCalendarUrl()}");
            }

            EventStateContainer.OnChange += StateHasChanged;
        }

        public void Dispose() => EventStateContainer.OnChange -= StateHasChanged;

        protected void DateChangedHandler(DateTime newDateValue) => NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");

        protected void ValueChangedHandler(DateTime newDateValue) => NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");

        protected void AddEvent() => NavigationManager.NavigateTo($"/calendar/create");
        protected void EditEvent(EventViewModel selectedEvent) => NavigationManager.NavigateTo($"/calendar/{selectedEvent.Id}");
    }
}
