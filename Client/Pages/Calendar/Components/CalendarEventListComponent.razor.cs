using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couple.Client.Adapters;
using Couple.Client.Model.Calendar;
using Couple.Client.States.Calendar;
using Couple.Client.Utility;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Calendar.Components
{
    public partial class CalendarEventListComponent
    {
        [Inject] private EventStateContainer EventStateContainer { get; init; }
        [Inject] private SelectedDateStateContainer SelectedDateStateContainer { get; init; }
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public string Style { get; set; }
        [Parameter] public Action OnSwipeUpCallback { get; init; }
        [Parameter] public Action OnSwipeDownCallback { get; init; }

        private VerticalSwipeHandler _verticalSwipeHandler;

        protected override async Task OnInitializedAsync()
        {
            _verticalSwipeHandler = new(SwipeUp, SwipeDown);

            EventStateContainer.OnChange += StateHasChanged;

            var events = await Js.InvokeAsync<List<EventModel>>("getAllEvents");
            EventStateContainer.SetEvents(events);
        }

        private ICollection<EventViewModel> Events =>
            EventStateContainer.TryGetEvents(SelectedDateStateContainer.SelectedDate, out var events)
                ? EventAdapter.ToViewModel(events)
                : new();

        private void EditEvent(EventViewModel selectedEvent) =>
            NavigationManager.NavigateTo($"/calendar/{selectedEvent.Id}");

        private void SwipeUp() => OnSwipeUpCallback.Invoke();
        private void SwipeDown() => OnSwipeDownCallback.Invoke();

        public void Dispose() => EventStateContainer.OnChange -= StateHasChanged;
    }
}
