using Couple.Client.Adapters;
using Couple.Client.Infrastructure;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.Issue;
using Couple.Client.States.Calendar;
using Couple.Client.States.Issue;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Event;
using Couple.Shared.Model.Issue;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Couple.Client.Services
{
    public class Synchronizer
    {
        private readonly IJSRuntime _js;

        private readonly HttpClient _httpClient;

        private readonly IssueStateContainer _issueStateContainer;
        private readonly EventStateContainer _eventStateContainer;

        private readonly JsonSerializerOptions _options = new() {PropertyNameCaseInsensitive = true};

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
            foreach (var item in toSynchronize)
            {
                switch (item.Command)
                {
                    case Command.CreateIssue:
                        var createIssueDto = JsonSerializer.Deserialize<CreateIssueDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("addIssue", IssueAdapter.ToModel(createIssueDto));
                        break;
                    case Command.UpdateIssue:
                        var updateIssueDto = JsonSerializer.Deserialize<UpdateIssueDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("updateIssue", IssueAdapter.ToModel(updateIssueDto));
                        break;
                    case Command.DeleteIssue:
                        await _js.InvokeVoidAsync("deleteIssue",
                            JsonSerializer.Deserialize<Guid>(item.Content, _options));
                        break;
                    case Command.CompleteIssue:
                        var completeIssueDto = JsonSerializer.Deserialize<CompleteIssueDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("completeIssue", IssueAdapter.ToCompletedModel(completeIssueDto));
                        break;
                    case Command.CreateEvent:
                    {
                        var toCreate = JsonSerializer.Deserialize<CreateEventDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("addEvent",
                            EventAdapter.ToModel(toCreate.Event),
                            toCreate.Added);
                        break;
                    }
                    case Command.UpdateEvent:
                    {
                        var toUpdate = JsonSerializer.Deserialize<UpdateEventDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("updateEvent",
                            EventAdapter.ToModel(toUpdate.Event),
                            toUpdate.Added,
                            IssueAdapter.ToModel(toUpdate.Removed));
                        break;
                    }
                    case Command.DeleteEvent:
                    {
                        await _js.InvokeVoidAsync("removeEvent",
                            JsonSerializer.Deserialize<Guid>(item.Content, _options));
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var idsToDelete = new DeleteChangeDto
            {
                Guids = toSynchronize
                    .Select(change => change.Id)
                    .ToList(),
            };

            if (idsToDelete.Guids.Any())
            {
                await _httpClient.DeleteAsJsonAsync("api/Changes", idsToDelete);
            }

            var issues = _js.InvokeAsync<List<IssueModel>>("getIssues").AsTask();
            var eventsTask = _js.InvokeAsync<List<EventModel>>("getAllEvents").AsTask();
            await Task.WhenAll(issues, eventsTask);
            _issueStateContainer.Issues = issues.Result;
            _eventStateContainer.SetEvents(eventsTask.Result);
        }
    }
}
