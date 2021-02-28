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
    public partial class CreateEvent
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

        protected CreateEventViewModel ToCreate { get; set; }

        protected List<ToDoViewModel> Added { get; set; }

        private IJSObjectReference ToDoModule;
        private IJSObjectReference EventModule;

        protected override async Task OnInitializedAsync()
        {
            Added = new();
            ToCreate = new()
            {
                Title = "",
                Start = DateTime.Now,
                End = DateTime.Now,
                ToDos = new(),
            };

            ToDoModule = await Js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
            EventModule = await Js.InvokeAsync<IJSObjectReference>("import", "./Event.razor.js");
        }

        protected async Task Save()
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
                ToDos = Mapper.Map<List<ToDoModel>>(ToCreate.ToDos),
            };
            await EventModule.InvokeVoidAsync("add", toPersist, added, new List<ToDoModel>());

            var toDos = await ToDoModule.InvokeAsync<List<ToDoModel>>("getAll");
            ToDoStateContainer.ToDos = toDos;
            var events = await EventModule.InvokeAsync<List<EventModel>>("getAll");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo($"/calendar/{ToCreate.Start.ToCalendarUrl()}");

            var toCreate = new CreateEventDto
            {
                Event = Mapper.Map<EventDto>(toPersist),
                Added = added,
                Removed = new(),
            };
            await HttpClient.PostAsJsonAsync($"api/Events", toCreate);
        }

        protected bool IsEnabled => !string.IsNullOrWhiteSpace(ToCreate?.Title)
            && ToCreate.End >= ToCreate.Start
            && ToCreate.Start != DateTime.UnixEpoch
            && ToCreate.End != DateTime.UnixEpoch;

        protected void AddedChanged(List<ToDoViewModel> added)
        {
            ToCreate.ToDos.AddRange(added);
            Added = ToCreate.ToDos = new(ToCreate.ToDos); // https://docs.telerik.com/blazor-ui/common-features/observable-data
        }

        protected void RemovedChanged(ToDoViewModel removed)
        {
            ToCreate.ToDos.Remove(removed);
            Added = ToCreate.ToDos = new(ToCreate.ToDos);
        }
    }
}
