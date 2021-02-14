using Couple.Client.Data;
using Couple.Client.Data.Calendar;
using Couple.Client.Data.ToDo;
using Couple.Client.Infrastructure;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Couple.Shared.Model;
using Couple.Shared.Model.Calendar;
using Microsoft.AspNetCore.Components;
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
        protected LocalStore LocalStore { get; set; }

        [Inject]
        private ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        private EventStateContainer EventStateContainer { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public EventCallback OnSynchronisationCallback { get; set; }

        private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        protected async Task Synchronize()
        {
            var toSynchronize = await HttpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
            foreach (var item in toSynchronize)
            {
                if (item.DataType == DataType.ToDo && item.Function == Function.Create)
                {
                    await LocalStore.PutAsync("todo", JsonSerializer.Deserialize<ToDoModel>(item.Content, options));
                }
                else if (item.DataType == DataType.ToDo && item.Function == Function.Update)
                {
                    await LocalStore.PutAsync("todo", JsonSerializer.Deserialize<ToDoModel>(item.Content, options));
                }
                else if (item.DataType == DataType.ToDo && item.Function == Function.Delete)
                {
                    await LocalStore.DeleteAsync("todo", JsonSerializer.Deserialize<Guid>(item.Content, options));
                }
                else if (item.DataType == DataType.Calendar && item.Function == Function.Create)
                {
                    var toCreate = JsonSerializer.Deserialize<CreateEventDto>(item.Content, options);
                    await LocalStore.PutEventAsync(new EventModel
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
                    await LocalStore.PutEventAsync(new EventModel
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
                    await LocalStore.DeleteEventAsync(toDelete.Id,
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
            await ToDoStateContainer.RefreshAsync();
            await EventStateContainer.RefreshAsync();

            await OnSynchronisationCallback.InvokeAsync();
        }
    }
}
