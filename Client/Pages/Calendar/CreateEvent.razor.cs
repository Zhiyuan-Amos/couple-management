using Couple.Client.Adapters;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.ToDo;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.Calendar;
using Couple.Client.ViewModel.ToDo;
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
    public partial class CreateEvent
    {
        [Inject] private HttpClient HttpClient { get; init; }

        [Inject] private NavigationManager NavigationManager { get; init; }

        [Inject] private ToDoStateContainer ToDoStateContainer { get; init; }

        [Inject] private EventStateContainer EventStateContainer { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        private CreateEventViewModel ToCreate { get; set; }

        private List<ToDoViewModel> Added { get; set; }

        protected override void OnInitialized()
        {
            Added = new();
            ToCreate = new()
            {
                Title = "",
                Start = DateTime.Now,
                End = DateTime.Now,
                ToDos = new(),
            };
        }

        private async Task Save()
        {
            var id = Guid.NewGuid();
            var added = Added
                .Select(toDo => toDo.Id)
                .ToList();
            var toPersist = new EventModel
            {
                Id = id,
                Title = ToCreate.Title,
                Start = ToCreate.Start,
                End = ToCreate.End,
                ToDos = ToDoAdapter.ToModel(ToCreate.ToDos),
            };
            await Js.InvokeVoidAsync("addEvent", toPersist, added);

            var toDosTask = Js.InvokeAsync<List<ToDoModel>>("getToDos").AsTask();
            var eventsTask = Js.InvokeAsync<List<EventModel>>("getAllEvents").AsTask();
            await Task.WhenAll(toDosTask, eventsTask);
            ToDoStateContainer.ToDos = toDosTask.Result;
            EventStateContainer.SetEvents(eventsTask.Result);

            NavigationManager.NavigateTo("/calendar");

            var toCreate = new CreateEventDto
            {
                Event = EventAdapter.ToDto(toPersist),
                Added = added,
            };
            await HttpClient.PostAsJsonAsync($"api/Events", toCreate);
        }

        private bool IsEnabled => !string.IsNullOrWhiteSpace(ToCreate?.Title)
                                    && ToCreate.End >= ToCreate.Start
                                    && ToCreate.Start != DateTime.UnixEpoch
                                    && ToCreate.End != DateTime.UnixEpoch;

        private void AddedChanged(List<ToDoViewModel> added)
        {
            ToCreate.ToDos.AddRange(added);
            Added = ToCreate.ToDos = new(ToCreate.ToDos); // https://docs.telerik.com/blazor-ui/common-features/observable-data
        }

        private void RemovedChanged(ToDoViewModel removed)
        {
            ToCreate.ToDos.Remove(removed);
            Added = ToCreate.ToDos = new(ToCreate.ToDos);
        }
    }
}
