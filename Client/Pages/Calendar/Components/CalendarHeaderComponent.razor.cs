using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Calendar.Components
{
    public partial class CalendarHeaderComponent
    {
        [Parameter] public Action<double> AfterRenderCallback { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var calendarHeaderHeight = ((IJSInProcessRuntime)Js).Invoke<double>("getCalendarHeaderHeight");
                AfterRenderCallback.Invoke(calendarHeaderHeight);
            }
        }
    }
}
