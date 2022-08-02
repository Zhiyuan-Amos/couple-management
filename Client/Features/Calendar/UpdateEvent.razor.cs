using System.Net.Http.Json;
using Couple.Client.Features.Calendar.Adapter;
using Couple.Client.Features.Calendar.Models;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Calendar;

public class UpdateEventBase : CreateUpdateEventBase
{
    private IReadOnlyEventModel _currentEventModel = default!;
    [EditorRequired][Parameter] public Guid EventId { get; set; }

    protected override void OnInitialized()
    {
        if (!EventStateContainer.TryGetEvent(EventId, out _currentEventModel!))
        {
            NavigationManager.NavigateTo("/calendar");
            return;
        }

        CreateUpdateEventStateContainer = new(_currentEventModel.Title,
            _currentEventModel.For,
            _currentEventModel.Start,
            _currentEventModel.End);
    }

    protected async Task Delete()
    {
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        db.Events.Remove((_currentEventModel as EventModel)!);
        await db.SaveChangesAsync();
        EventStateContainer.DeleteEvent(_currentEventModel.Id);

        NavigationManager.NavigateTo("/calendar");

        await HttpClient.DeleteAsync($"api/Event/{EventId}");
    }

    protected override async Task Save()
    {
        var @event = new EventModel(_currentEventModel.Id, _currentEventModel.Title, _currentEventModel.For,
            _currentEventModel.Start, _currentEventModel.End, _currentEventModel.CreatedOn);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();

        db.Attach(@event);
        @event.Title = CreateUpdateEventStateContainer.Title;
        @event.For = CreateUpdateEventStateContainer.For;
        @event.Start = CreateUpdateEventStateContainer.Start;
        @event.End = CreateUpdateEventStateContainer.End;
        await db.SaveChangesAsync();

        EventStateContainer.UpdateEvent(@event);
        NavigationManager.NavigateTo("/calendar");

        var toUpdate = EventAdapter.ToUpdateDto(@event);
        await HttpClient.PutAsJsonAsync("api/Event", toUpdate);
    }
}
