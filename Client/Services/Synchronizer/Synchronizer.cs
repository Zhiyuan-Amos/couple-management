using Couple.Client.Infrastructure;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.Issue;
using Couple.Client.States.Calendar;
using Couple.Client.States.Issue;
using Couple.Shared.Model.Change;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Couple.Client.Services.Synchronizer;

public class Synchronizer
{
    private readonly EventStateContainer _eventStateContainer;

    private readonly HttpClient _httpClient;

    private readonly IssueStateContainer _issueStateContainer;
    private readonly IJSRuntime _js;

    public Synchronizer(IJSRuntime js,
        HttpClient httpClient,
        IssueStateContainer issueStateContainer,
        EventStateContainer eventStateContainer)
    {
        _js = js;
        _httpClient = httpClient;
        _issueStateContainer = issueStateContainer;
        _eventStateContainer = eventStateContainer;
    }

    public async Task SynchronizeAsync()
    {
        var toSynchronize = await _httpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
        var parser = new CommandParser(_js);

        foreach (var change in toSynchronize)
        {
            var command = parser.Parse(change);
            await command.Execute();
        }

        var idsToDelete = new DeleteChangeDto(toSynchronize
            .Select(change => change.Id)
            .ToList());

        if (idsToDelete.Guids.Any()) await _httpClient.DeleteAsJsonAsync("api/Changes", idsToDelete);

        var issues = _js.InvokeAsync<List<IssueModel>>("getIssues").AsTask();
        var eventsTask = _js.InvokeAsync<List<EventModel>>("getAllEvents").AsTask();
        await Task.WhenAll(issues, eventsTask);
        _issueStateContainer.Issues = issues.Result;
        _eventStateContainer.SetEvents(eventsTask.Result);
    }
}
