using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Calendar.Components
{
    public partial class CalendarTopBar
    {
        [Parameter]
        public EventCallback OnSynchronisationCallback { get; init; }
    }
}

