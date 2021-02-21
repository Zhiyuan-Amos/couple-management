using AutoMapper;
using Couple.Client.Infrastructure;
using Couple.Client.Model.Calendar;
using Couple.Client.States.Calendar;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Linq;
>>>>>>> 3ff24a5... Initialize fullCalendar
using System.Threading.Tasks;

namespace Couple.Client.Pages.Calendar
{
    public partial class Calendar
    {
        private ElementReference fullCalendar;

        [Inject]
        private NavigationManager NavigationManager { get; init; }

        [Inject]
        private EventStateContainer EventStateContainer { get; init; }

<<<<<<< HEAD
        [Inject]
        private IMapper Mapper { get; init; }

        [Inject]
        private IJSRuntime Js { get; init; }

=======
>>>>>>> 3ff24a5... Initialize fullCalendar
        [Parameter]
        public DateTime Selected { get; set; }

        protected IEnumerable<EventViewModel> Events => EventStateContainer.TryGetEvents(Selected, out var events)
            ? Mapper.Map<List<EventViewModel>>(events)
            : new();

        protected override async Task OnInitializedAsync()
        {
            if (Selected == new DateTime())
            {
                Selected = DateTime.Now.Date;
                NavigationManager.NavigateTo($"/calendar/{Selected.ToCalendarUrl()}");
            }

            EventStateContainer.OnChange += StateHasChanged;

<<<<<<< HEAD
            var events = await Js.InvokeAsync<List<EventModel>>("getAllEvents");
            EventStateContainer.SetEvents(events);
=======
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync(
                    "intializeCalendar", fullCalendar);
            }
        }

        protected void DateChangedHandler(DateTime newDateValue)
        {
            NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");
            Events = GetEvents(newDateValue);
>>>>>>> 3ff24a5... Initialize fullCalendar
        }

        public void Dispose() => EventStateContainer.OnChange -= StateHasChanged;

        protected void DateChangedHandler(DateTime newDateValue) => NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");

        protected void ValueChangedHandler(DateTime newDateValue) => NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");

<<<<<<< HEAD
        protected void AddEvent() => NavigationManager.NavigateTo($"/calendar/create");
        protected void EditEvent(EventViewModel selectedEvent) => NavigationManager.NavigateTo($"/calendar/{selectedEvent.Id}");
=======
        protected void RefreshEvents() => Events = GetEvents(Selected);

        private async Task GetCalendar()
        {
            await JS.InvokeVoidAsync("intializeCalendar");
        }
>>>>>>> 3ff24a5... Initialize fullCalendar
    }
}
