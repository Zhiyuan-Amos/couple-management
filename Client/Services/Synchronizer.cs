﻿using AutoMapper;
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
        private readonly IJSRuntime _js;

        private readonly HttpClient _httpClient;

        private readonly ToDoStateContainer _toDoStateContainer;
        private readonly EventStateContainer _eventStateContainer;

        private readonly IMapper _mapper;

        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        private readonly Task _initialSynchronization;

        // See https://blog.stephencleary.com/2013/01/async-oop-2-constructors.html
        public Synchronizer(IJSRuntime js,
                            HttpClient httpClient,
                            ToDoStateContainer toDoStateContainer,
                            EventStateContainer eventStateContainer,
                            IMapper mapper)
        {
            _js = js;
            _httpClient = httpClient;
            _toDoStateContainer = toDoStateContainer;
            _eventStateContainer = eventStateContainer;
            _mapper = mapper;
            _initialSynchronization = Task.Delay(10000).ContinueWith(_ => SynchronizeAsync());
        }

        public async Task SynchronizeAsync()
        {
            var toSynchronize = await _httpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
            foreach (var item in toSynchronize)
            {
                switch (item.DataType)
                {
                    case DataType.ToDo when item.Function == Function.Create:
                        await _js.InvokeVoidAsync("addToDo", JsonSerializer.Deserialize<ToDoModel>(item.Content, _options));
                        break;
                    case DataType.ToDo when item.Function == Function.Update:
                        await _js.InvokeVoidAsync("updateToDo", JsonSerializer.Deserialize<ToDoModel>(item.Content, _options));
                        break;
                    case DataType.ToDo when item.Function == Function.Delete:
                        await _js.InvokeVoidAsync("removeToDo", JsonSerializer.Deserialize<Guid>(item.Content, _options));
                        break;
                    case DataType.Calendar when item.Function == Function.Create:
                    {
                        var toCreate = JsonSerializer.Deserialize<CreateEventDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("addEvent",
                            _mapper.Map<EventModel>(toCreate.Event),
                            toCreate.Added);
                        break;
                    }
                    case DataType.Calendar when item.Function == Function.Update:
                    {
                        var toUpdate = JsonSerializer.Deserialize<UpdateEventDto>(item.Content, _options);
                        await _js.InvokeVoidAsync("updateEvent",
                            _mapper.Map<EventModel>(toUpdate.Event),
                            toUpdate.Added,
                            _mapper.Map<List<ToDoModel>>(toUpdate.Removed));
                        break;
                    }
                    case DataType.Calendar when item.Function == Function.Delete:
                    {
                        await _js.InvokeVoidAsync("removeEvent", JsonSerializer.Deserialize<Guid>(item.Content, _options));
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

            var toDosTask = _js.InvokeAsync<List<ToDoModel>>("getAllToDos").AsTask();
            var eventsTask = _js.InvokeAsync<List<EventModel>>("getAllEvents").AsTask();
            await Task.WhenAll(toDosTask, eventsTask);
            _toDoStateContainer.ToDos = toDosTask.Result;
            _eventStateContainer.SetEvents(eventsTask.Result);
        }
    }
}