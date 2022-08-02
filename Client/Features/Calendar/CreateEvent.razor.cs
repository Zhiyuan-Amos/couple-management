using System.Net.Http.Json;
using Couple.Client.Features.Calendar.Adapter;
using Couple.Client.Features.Calendar.Models;
using Couple.Shared.Models;

namespace Couple.Client.Features.Calendar;

public class CreateEventBase : CreateUpdateEventBase
{
    protected override void OnInitialized() =>
        CreateUpdateEventStateContainer = new("",
            For.Him,
            DateTime.Now,
            DateTime.Now.AddHours(1));

    protected override async Task Save()
    {
        var toPersist = new EventModel(CreateUpdateEventStateContainer.Title,
            CreateUpdateEventStateContainer.For,
            CreateUpdateEventStateContainer.Start,
            CreateUpdateEventStateContainer.End,
            DateTime.Now);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();

        db.Events.Add(toPersist);
        await db.SaveChangesAsync();

        EventStateContainer.AddEvent(toPersist);
        NavigationManager.NavigateTo("/calendar");

        var toCreate = EventAdapter.ToCreateDto(toPersist);
        await HttpClient.PostAsJsonAsync("api/Event", toCreate);
    }
}
