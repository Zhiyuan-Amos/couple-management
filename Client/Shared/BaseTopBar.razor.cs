using Couple.Client.Data.Calendar;
using Couple.Client.Data.ToDo;
using Couple.Client.Infrastructure;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Couple.Shared.Model;
using Couple.Shared.Model.Calendar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Couple.Client.Shared
{
    public partial class BaseTopBar
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        private EventStateContainer EventStateContainer { get; set; }

        [Inject]
        private IJSRuntime Js { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public EventCallback OnSynchronisationCallback { get; set; }

        private IJSObjectReference ToDoModule;
        private IJSObjectReference EventModule;

        private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        protected override async Task OnInitializedAsync()
        {
            ToDoModule = await Js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
            EventModule = await Js.InvokeAsync<IJSObjectReference>("import", "./Event.razor.js");
        }

        protected async Task Synchronize()
        {
            var toSynchronize = await HttpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
            foreach (var item in toSynchronize)
            {
                if (item.DataType == DataType.ToDo && item.Function == Function.Create)
                {
                    await ToDoModule.InvokeVoidAsync("add", JsonSerializer.Deserialize<ToDoModel>(item.Content, options));
                }
                else if (item.DataType == DataType.ToDo && item.Function == Function.Update)
                {
                    await ToDoModule.InvokeVoidAsync("update", JsonSerializer.Deserialize<ToDoModel>(item.Content, options));
                }
                else if (item.DataType == DataType.ToDo && item.Function == Function.Delete)
                {
                    await ToDoModule.InvokeVoidAsync("remove", JsonSerializer.Deserialize<Guid>(item.Content, options));
                }
                else if (item.DataType == DataType.Calendar && item.Function == Function.Create)
                {
                    var toCreate = JsonSerializer.Deserialize<CreateEventDto>(item.Content, options);
                    await EventModule.InvokeVoidAsync("add", new EventModel
                    {
                        Id = toCreate.Event.Id,
                        Title = toCreate.Event.Title,
                        Start = toCreate.Event.Start,
                        End = toCreate.Event.End,
                        ToDos = toCreate.Event.ToDos
                        .Select(toDo => new ToDoModel
                        {
                            Id = toDo.Id,
                            Text = toDo.Text,
                            Category = toDo.Category,
                            CreatedOn = toDo.CreatedOn
                        }).ToList(),
                    },
                    toCreate.Added,
                    toCreate.Removed.Select(toDo => new ToDoModel
                    {
                        Id = toDo.Id,
                        Text = toDo.Text,
                        Category = toDo.Category,
                        CreatedOn = toDo.CreatedOn,
                    }));
                }
                else if (item.DataType == DataType.Calendar && item.Function == Function.Update)
                {
                    var toUpdate = JsonSerializer.Deserialize<UpdateEventDto>(item.Content, options);
                    await EventModule.InvokeVoidAsync("update", new EventModel
                    {
                        Id = toUpdate.Event.Id,
                        Title = toUpdate.Event.Title,
                        Start = toUpdate.Event.Start,
                        End = toUpdate.Event.End,
                        ToDos = toUpdate.Event.ToDos
                        .Select(toDo => new ToDoModel
                        {
                            Id = toDo.Id,
                            Text = toDo.Text,
                            Category = toDo.Category,
                            CreatedOn = toDo.CreatedOn
                        }).ToList(),
                    },
                    toUpdate.Added,
                    toUpdate.Removed.Select(toDo => new ToDoModel
                    {
                        Id = toDo.Id,
                        Text = toDo.Text,
                        Category = toDo.Category,
                        CreatedOn = toDo.CreatedOn,
                    }));
                }
                else if (item.DataType == DataType.Calendar && item.Function == Function.Delete)
                {
                    var toDelete = JsonSerializer.Deserialize<DeleteEventDto>(item.Content, options);
                    await EventModule.InvokeVoidAsync("remove",
                        toDelete.Id,
                        toDelete.Removed.Select(toDo => new ToDoModel
                        {
                            Id = toDo.Id,
                            Text = toDo.Text,
                            Category = toDo.Category,
                            CreatedOn = toDo.CreatedOn,
                        }));
                }
            }

            var idsToDelete = new DeleteChangeDto
            {
                Guids = toSynchronize
                    .Select(change => change.Id)
                    .ToList(),
            };

            await HttpClient.DeleteAsJsonAsync($"api/Changes", idsToDelete);
            var toDos = await ToDoModule.InvokeAsync<List<ToDoModel>>("getAll");
            ToDoStateContainer.ToDos = toDos;
            var events = await EventModule.InvokeAsync<List<EventModel>>("getAll");
            EventStateContainer.SetEvents(events);

            await OnSynchronisationCallback.InvokeAsync();
        }
    }
}
