using AutoMapper;
using Couple.Client.Infrastructure;
using Couple.Client.Model.Calendar;
using Couple.Client.States.Calendar;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [Inject]
        private IJSRuntime Js { get; init; }

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var events = await Js.InvokeAsync<List<EventModel>>("getAllEvents");
                EventStateContainer.SetEvents(events);
            }
        }

        public void Dispose() => EventStateContainer.OnChange -= StateHasChanged;

        protected void DateChangedHandler(DateTime newDateValue) => NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");

        protected void ValueChangedHandler(DateTime newDateValue) => NavigationManager.NavigateTo($"/calendar/{newDateValue.ToCalendarUrl()}");

        protected void AddEvent() => NavigationManager.NavigateTo($"/calendar/create");
        protected void EditEvent(EventViewModel selectedEvent) => NavigationManager.NavigateTo($"/calendar/{selectedEvent.Id}");
    }
}
