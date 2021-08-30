using Couple.Client.Adapters;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.Issue;
using Couple.Client.States.Calendar;
using Couple.Client.States.Issue;
using Couple.Client.ViewModel.Calendar;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model.Event;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Calendar
{
    public partial class UpdateEvent
    {
        [Inject] private HttpClient HttpClient { get; init; }

        [Inject] private NavigationManager NavigationManager { get; init; }

        [Inject] private IssueStateContainer IssueStateContainer { get; init; }

        [Inject] private EventStateContainer EventStateContainer { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public Guid EventId { get; set; }

        private UpdateEventViewModel ToUpdate { get; set; }

        private List<IssueViewModel> Original { get; set; }
        private List<IssueViewModel> Added { get; set; }
        private List<IssueViewModel> Removed { get; set; }

        protected override void OnInitialized()
        {
            if (!EventStateContainer.TryGetEvent(EventId, out var @event))
            {
                NavigationManager.NavigateTo("/calendar");
                return;
            }

            Original = IssueAdapter.ToViewModel(@event.ToDos);
            Added = new();
            Removed = new();

            ToUpdate = EventAdapter.ToUpdateViewModel(@event);
        }

        private async Task Save()
        {
            var added = Added.Select(toDo => toDo.Id).ToList();
            var toPersist = EventAdapter.ToModel(ToUpdate);
            await Js.InvokeVoidAsync("updateEvent", new object[]
            {
                toPersist,
                added,
                IssueAdapter.ToModel(Removed)
            });

            var toDosTask = Js.InvokeAsync<List<IssueModel>>("getToDos").AsTask();
            var eventsTask = Js.InvokeAsync<List<EventModel>>("getAllEvents").AsTask();
            await Task.WhenAll(toDosTask, eventsTask);
            IssueStateContainer.Issues = toDosTask.Result;
            EventStateContainer.SetEvents(eventsTask.Result);

            NavigationManager.NavigateTo("/calendar");

            var toUpdate = new UpdateEventDto
            {
                Event = EventAdapter.ToDto(ToUpdate),
                Added = added,
                Removed = IssueAdapter.ToDto(Removed),
            };
            await HttpClient.PutAsJsonAsync($"api/Events", toUpdate);
        }

        private async Task Delete()
        {
            await Js.InvokeVoidAsync("removeEvent", ToUpdate.Id);
            var events = await Js.InvokeAsync<List<EventModel>>("getAllEvents");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo("/calendar");

            await HttpClient.DeleteAsync($"api/Events/{ToUpdate.Id}");
        }

        private bool IsEnabled => !string.IsNullOrWhiteSpace(ToUpdate?.Title)
                                    && ToUpdate.End >= ToUpdate.Start
                                    && ToUpdate.Start != DateTime.UnixEpoch
                                    && ToUpdate.End != DateTime.UnixEpoch;

        private void AddedChanged(List<IssueViewModel> added)
        {
            foreach (var add in added)
            {
                if (Original.Any(toDo => toDo.Id == add.Id))
                {
                    Removed.Remove(add);
                }
                else
                {
                    Added.Add(add);
                }
            }

            ToUpdate.ToDos.AddRange(added);
            ToUpdate.ToDos = new(ToUpdate.ToDos); // https://docs.telerik.com/blazor-ui/common-features/observable-data
        }

        private void RemovedChanged(IssueViewModel removed)
        {
            if (Original.Any(toDo => toDo.Id == removed.Id))
            {
                Removed.Add(removed);
            }
            else
            {
                Added.Remove(removed);
            }

            ToUpdate.ToDos.Remove(removed);
            ToUpdate.ToDos = new(ToUpdate.ToDos);
        }
    }
}
