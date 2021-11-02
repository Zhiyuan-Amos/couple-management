using System;
using System.Collections.Generic;

namespace Couple.Client.States.Done
{
    public class DoneStateContainer : Notifier
    {
        private SortedDictionary<DateOnly, List<object>> _dateToItems = new();

        public IReadOnlyDictionary<DateOnly, List<object>> GetDateToItems() => _dateToItems;

        public void SetDateToItems(IDictionary<DateOnly, List<object>> toSet)
        {
            _dateToItems = new(toSet);
            NotifyStateChanged();
        }
    }
}
