using Couple.Client.Features.Calendar.Models;
using Couple.Client.Features.Calendar.States;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Features.Calendar.Components;

public partial class EventListView : IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;

    [Inject] private EventStateContainer EventStateContainer { get; init; } = default!;
    
    public void Dispose() => EventStateContainer.OnChange -= StateHasChanged;
    
    private List<IReadOnlyEventModel> Events =>
        EventStateContainer.Events
            .OrderByDescending(@event => @event.Start)
            .ToList();

    protected override void OnInitialized() =>
        EventStateContainer.OnChange += StateHasChanged;

    private void EditEvent(IReadOnlyEventModel selectedIssue) =>
        NavigationManager.NavigateTo($"/calendar/{selectedIssue.Id}");

    private string ToReadableDateTime(DateTime dt) => dt.ToString("dd/MM hh:mm");
}
