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
    public partial class UpdateEvent
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

        [Parameter]
        public Guid EventId { get; set; }

        protected UpdateEventModel ToUpdate { get; set; }

        protected List<ToDoViewModel> Original { get; set; }
        protected List<ToDoViewModel> Added { get; set; }
        protected List<ToDoViewModel> Removed { get; set; }

        protected override void OnInitialized()
        {
            if (!EventStateContainer.TryGetEvent(EventId, out var @event))
            {
                NavigationManager.NavigateTo("calendar");
                return;
            }

            Original = new(@event.ToDos.Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn)));
            Added = new();
            Removed = new();

            ToUpdate = new()
            {
                Id = @event.Id,
                Title = @event.Title,
                Start = @event.Start,
                End = @event.End,
                ToDos = new(@event.ToDos.Select(toDo => new ToDoViewModel(toDo.Id, toDo.Text, toDo.Category, toDo.CreatedOn))),
            };
        }

        protected async Task Save()
        {
            var added = Added.Select(toDo => toDo.Id).ToList();
            var toPersist = new EventModel
            {
                Id = ToUpdate.Id,
                Title = ToUpdate.Title,
                Start = ToUpdate.Start,
                End = ToUpdate.End,
                ToDos = ToUpdate.ToDos
                    .Select(toDo => new ToDoModel
                    {
                        Id = toDo.Id,
                        Category = toDo.Category,
                        Text = toDo.Text,
                        CreatedOn = toDo.CreatedOn
                    }).ToList(),
            };
            await LocalStore.PutEventAsync(toPersist,
                added,
                Removed.Select(toDo => new ToDoModel
                {
                    Id = toDo.Id,
                    Text = toDo.Text,
                    Category = toDo.Category,
                    CreatedOn = toDo.CreatedOn,
                }).ToList());

            var toDos = await LocalStore.GetAllAsync<List<ToDoModel>>("todo");
            ToDoStateContainer.SetToDos(toDos);
            var events = await LocalStore.GetAllAsync<List<EventModel>>("event");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo($"/calendar/{ToUpdate.Start.ToCalendarUrl()}");

            var toUpdate = new UpdateEventDto
            {
                Event = new EventDto
                {
                    Id = ToUpdate.Id,
                    Title = ToUpdate.Title,
                    Start = ToUpdate.Start,
                    End = ToUpdate.End,
                    ToDos = ToUpdate.ToDos.Select(toDo => new ToDoDto
                    {
                        Id = toDo.Id,
                        Text = toDo.Text,
                        Category = toDo.Category,
                        CreatedOn = toDo.CreatedOn,
                    }).ToList(),
                },
                Added = added,
                Removed = Removed.Select(toDo => new ToDoDto
                {
                    Id = toDo.Id,
                    Text = toDo.Text,
                    Category = toDo.Category,
                    CreatedOn = toDo.CreatedOn,
                }).ToList(),
            };
            await HttpClient.PutAsJsonAsync($"api/Events", toUpdate);
        }

        protected async Task Delete()
        {
            await LocalStore.DeleteEventAsync(
                ToUpdate.Id,
                ToUpdate
                    .ToDos
                    .Select(toDo => new ToDoModel
                    {
                        Id = toDo.Id,
                        Text = toDo.Text,
                        Category = toDo.Category,
                        CreatedOn = toDo.CreatedOn,
                    }).ToList());
            var events = await LocalStore.GetAllAsync<List<EventModel>>("event");
            EventStateContainer.SetEvents(events);

            NavigationManager.NavigateTo($"/calendar/{ToUpdate.Start.ToCalendarUrl()}");

            var toDelete = new DeleteEventDto
            {
                Id = ToUpdate.Id,
                Removed = ToUpdate.ToDos.Select(toDo => new ToDoDto
                {
                    Id = toDo.Id,
                    Text = toDo.Text,
                    Category = toDo.Category,
                    CreatedOn = toDo.CreatedOn,
                }).ToList(),
            };

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
