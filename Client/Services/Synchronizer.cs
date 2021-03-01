using Couple.Client.Infrastructure;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.ToDo;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Event;
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
        private readonly HttpClient _httpClient;

        private readonly ToDoStateContainer _toDoStateContainer;
        private readonly EventStateContainer _eventStateContainer;

        private IJSObjectReference _toDoModule;
        private IJSObjectReference _eventModule;

        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        // See https://blog.stephencleary.com/2013/01/async-oop-2-constructors.html
        public Synchronizer(IJSRuntime js,
                            HttpClient httpClient,
                            ToDoStateContainer toDoStateContainer,
                            EventStateContainer eventStateContainer)
        {
            _httpClient = httpClient;
            _toDoStateContainer = toDoStateContainer;
            _eventStateContainer = eventStateContainer;
            Initialization = InitializeAsync(js);
        }

        public Task Initialization { get; }

        private async Task InitializeAsync(IJSRuntime js)
        {
            _toDoModule = await js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
            _eventModule = await js.InvokeAsync<IJSObjectReference>("import", "./Event.razor.js");
        }

        public async Task SynchronizeAsync()
        {
            var toSynchronize = await _httpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
            foreach (var item in toSynchronize)
            {
                switch (item.DataType)
                {
                    case DataType.ToDo when item.Function == Function.Create:
                        await _toDoModule.InvokeVoidAsync("add", JsonSerializer.Deserialize<ToDoModel>(item.Content, _options));
                        break;
                    case DataType.ToDo when item.Function == Function.Update:
                        await _toDoModule.InvokeVoidAsync("update", JsonSerializer.Deserialize<ToDoModel>(item.Content, _options));
                        break;
                    case DataType.ToDo when item.Function == Function.Delete:
                        await _toDoModule.InvokeVoidAsync("remove", JsonSerializer.Deserialize<Guid>(item.Content, _options));
                        break;
                    case DataType.Calendar when item.Function == Function.Create:
                    {
                        var toCreate = JsonSerializer.Deserialize<CreateEventDto>(item.Content, _options);
                        await _eventModule.InvokeVoidAsync("add",
                            new EventModel
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
                            toCreate.Added);
                        break;
                    }
                    case DataType.Calendar when item.Function == Function.Update:
                    {
                        var toUpdate = JsonSerializer.Deserialize<UpdateEventDto>(item.Content, _options);
                        await _eventModule.InvokeVoidAsync("update", new EventModel
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
                        break;
                    }
                    case DataType.Calendar when item.Function == Function.Delete:
                    {
                        var toDelete = JsonSerializer.Deserialize<DeleteEventDto>(item.Content, _options);
                        await _eventModule.InvokeVoidAsync("remove",
                            toDelete.Id,
                            toDelete.Removed.Select(toDo => new ToDoModel
                            {
                                Id = toDo.Id,
                                Text = toDo.Text,
                                Category = toDo.Category,
                                CreatedOn = toDo.CreatedOn,
                            }));
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
                await _httpClient.DeleteAsJsonAsync($"api/Changes", idsToDelete);
            }

            var toDos = await _toDoModule.InvokeAsync<List<ToDoModel>>("getAll");
            _toDoStateContainer.ToDos = toDos;
            var events = await _eventModule.InvokeAsync<List<EventModel>>("getAll");
            _eventStateContainer.SetEvents(events);
        }
    }
}
