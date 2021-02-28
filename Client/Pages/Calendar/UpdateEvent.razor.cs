using AutoMapper;
using Couple.Client.Infrastructure;
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
    public partial class UpdateEvent
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        private EventStateContainer EventStateContainer { get; set; }

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private IJSRuntime Js { get; set; }

        [Parameter]
        public Guid EventId { get; set; }

        protected UpdateEventViewModel ToUpdate { get; set; }

        protected List<ToDoViewModel> Original { get; set; }
        protected List<ToDoViewModel> Added { get; set; }
        protected List<ToDoViewModel> Removed { get; set; }

        private IJSObjectReference ToDoModule;
        private IJSObjectReference EventModule;

        protected override async Task OnInitializedAsync()
        {
            if (!EventStateContainer.TryGetEvent(EventId, out var @event))
            {
                NavigationManager.NavigateTo("calendar");
                return;
            }

            Original = Mapper.Map<List<ToDoViewModel>>(@event.ToDos);
            Added = new();
            Removed = new();

            ToUpdate = Mapper.Map<UpdateEventViewModel>(@event);

            ToDoModule = await Js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
            EventModule = await Js.InvokeAsync<IJSObjectReference>("import", "./Event.razor.js");
        }

        protected async Task Save()
        {
            var added = Added.Select(toDo => toDo.Id).ToList();
            var toPersist = Mapper.Map<EventModel>(ToUpdate);
            await EventModule.InvokeVoidAsync("update",
                toPersist,
                added,
                Mapper.Map<List<ToDoModel>>(Removed));

            var toDos = await ToDoModule.InvokeAsync<List<ToDoModel>>("getAll");
            ToDoStateContainer.ToDos = toDos;
            var events = await EventModule.InvokeAsync<List<EventModel>>("getAll");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo($"/calendar/{ToUpdate.Start.ToCalendarUrl()}");

            var toUpdate = new UpdateEventDto
            {
                Event = Mapper.Map<EventDto>(ToUpdate),
                Added = added,
                Removed = Mapper.Map<List<ToDoDto>>(Removed),
            };
            await HttpClient.PutAsJsonAsync($"api/Events", toUpdate);
        }

        protected async Task Delete()
        {
            await EventModule.InvokeVoidAsync("remove",
                ToUpdate.Id,
                Mapper.Map<List<ToDoModel>>(ToUpdate.ToDos));
            var events = await EventModule.InvokeAsync<List<EventModel>>("getAll");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo($"/calendar/{ToUpdate.Start.ToCalendarUrl()}");

            var toDelete = Mapper.Map<DeleteEventDto>(ToUpdate);

            await HttpClient.DeleteAsJsonAsync($"api/Events", toDelete);
        }

        protected bool IsEnabled => !string.IsNullOrWhiteSpace(ToUpdate?.Title)
            && ToUpdate.End >= ToUpdate.Start
            && ToUpdate.Start != DateTime.UnixEpoch
            && ToUpdate.End != DateTime.UnixEpoch;

        protected void AddedChanged(List<ToDoViewModel> added)
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

        protected void RemovedChanged(ToDoViewModel removed)
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
