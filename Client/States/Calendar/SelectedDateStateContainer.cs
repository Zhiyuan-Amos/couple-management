using System;

namespace Couple.Client.States.Calendar
{
    public class SelectedDateStateContainer : Notifier
    {
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                NotifyStateChanged();
            }
        }
    }
}
