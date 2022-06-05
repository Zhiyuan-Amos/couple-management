using Couple.Client.Features.Calendar.States;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Features.Calendar;

public partial class Calendar
{
    private static bool s_isDataLoaded;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    
    [Inject] private EventStateContainer EventStateContainer { get; init; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        if (s_isDataLoaded)
        {
            return;
        }

        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        EventStateContainer.Events = await db.Events.ToListAsync();

        s_isDataLoaded = true;
    }

    private void AddEvent() => NavigationManager.NavigateTo("/calendar/create");
}
