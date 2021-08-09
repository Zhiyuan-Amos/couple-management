using Couple.Client.Adapters;
using Couple.Client.Infrastructure;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.ToDo;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Event;
using Couple.Shared.Model.ToDo;
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

        private readonly ToDoStateContainer _toDoStateContainer;
        private readonly EventStateContainer _eventStateContainer;

        private readonly JsonSerializerOptions _options = new() {PropertyNameCaseInsensitive = true};

        public Synchronizer(IJSRuntime js,
            HttpClient httpClient,
            ToDoStateContainer toDoStateContainer,
            EventStateContainer eventStateContainer)
        {
            _js = js;
            _httpClient = httpClient;
            _toDoStateContainer = toDoStateContainer;
            _eventStateContainer = eventStateContainer;
        }

        public async Task SynchronizeAsync()
        {
            var toSynchronize = await _httpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
            foreach (var item in toSynchronize)
            {
                switch (item.Command)
                {
                    case Command.CreateToDo:
                        var createToDoDto = JsonSerializer.Deserialize<CreateToDoDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("addToDo", ToDoAdapter.ToModel(createToDoDto));
                        break;
                    case Command.UpdateToDo:
                        var updateToDoDto = JsonSerializer.Deserialize<UpdateToDoDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("updateToDo", ToDoAdapter.ToModel(updateToDoDto));
                        break;
                    case Command.DeleteToDo:
                        await _js.InvokeVoidAsync("removeToDo",
                            JsonSerializer.Deserialize<Guid>(item.Content, _options));
                        break;
                    case Command.CompleteToDo:
                        var completeToDoDto = JsonSerializer.Deserialize<CompleteToDoDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("completeToDo", ToDoAdapter.ToCompletedModel(completeToDoDto));
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
                            ToDoAdapter.ToModel(toUpdate.Removed));
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

            var toDosTask = _js.InvokeAsync<List<ToDoModel>>("getToDos").AsTask();
            var eventsTask = _js.InvokeAsync<List<EventModel>>("getAllEvents").AsTask();
            await Task.WhenAll(toDosTask, eventsTask);
            _toDoStateContainer.ToDos = toDosTask.Result;
            _eventStateContainer.SetEvents(eventsTask.Result);
        }
    }
}
