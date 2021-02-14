using Couple.Client.Data;
using Couple.Client.Data.Calendar;
using Couple.Client.Data.ToDo;
using Couple.Client.Infrastructure;
using Couple.Client.Model.Event;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model.Calendar;
using Microsoft.AspNetCore.Components;
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
        protected LocalStore LocalStore { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        private EventStateContainer EventStateContainer { get; set; }

        protected CreateEventModel ToCreate { get; set; }

        protected List<ToDoViewModel> Added { get; set; }

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
                ToDos = ToCreate.ToDos
                    .Select(toDo => new ToDoModel
                    {
                        Id = toDo.Id,
                        Text = toDo.Text,
                        Category = toDo.Category,
                        CreatedOn = toDo.CreatedOn
                    }).ToList(),
            };
            await LocalStore.PutEventAsync(toPersist, added, new List<ToDoModel>());

            var toDos = await LocalStore.GetAllAsync<List<ToDoModel>>("todo");
            ToDoStateContainer.SetToDos(toDos);
            var events = await LocalStore.GetAllAsync<List<EventModel>>("event");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo($"/calendar/{ToCreate.Start.ToCalendarUrl()}");

            var toCreate = new CreateEventDto
            {
                Event = new EventDto
                {
                    Id = id,
                    Title = ToCreate.Title,
                    Start = ToCreate.Start,
                    End = ToCreate.End,
                    ToDos = ToCreate.ToDos.Select(toDo => new ToDoDto
                    {
                        Id = toDo.Id,
                        Text = toDo.Text,
                        Category = toDo.Category,
                        CreatedOn = toDo.CreatedOn,
                    }).ToList(),
                },
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
