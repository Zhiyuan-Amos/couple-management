using System;
using System.Collections.Generic;
using Couple.Client.States.Calendar;
using Couple.Client.ViewModel.Calendar;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Calendar.Components
{
    public partial class CalendarBodyComponent
    {
        [Inject] private SelectedDateStateContainer SelectedDateStateContainer { get; init; }

        [Parameter] public List<CellViewModel> Cells { get; set; }

        private const int WeeksOnCalendar = 6;
        private const int DaysOfWeek = 7;

        private DateTime _selected;

        protected override void OnInitialized()
        {
            _selected = SelectedDateStateContainer.SelectedDate;
            SelectedDateStateContainer.OnChange += SelectedDateChangedHandler;
        }

        private void SelectionHandler(DateTime selected) => SelectedDateStateContainer.SelectedDate = selected;

        private void SelectedDateChangedHandler() => _selected = SelectedDateStateContainer.SelectedDate;

        public void Dispose() => SelectedDateStateContainer.OnChange -= SelectedDateChangedHandler;
    }
}
