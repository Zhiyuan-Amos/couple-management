using Microsoft.AspNetCore.Components;

namespace Couple.Client.Components.Calendar
{
    public partial class CalendarTopBar
    {
        [Parameter]
        public EventCallback OnSynchronisationCallback { get; set; }
    }
}

