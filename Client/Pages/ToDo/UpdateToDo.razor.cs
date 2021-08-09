using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public class UpdateToDoBase : CreateUpdateToDoBase
    {
        [Parameter] public Guid ToDoId { get; set; }

        protected override void OnInitialized()
        {
            if (!ToDoStateContainer.TryGetToDo(ToDoId, out var toDo))
            {
                NavigationManager.NavigateTo("/todo");
                return;
            }

            CreateUpdateToDoStateContainer.Initialize(toDo.Id,
                toDo.Name,
                toDo.For,
                toDo.ToDos,
                toDo.CreatedOn);
        }

        protected async Task Delete()
        {
            var id = CreateUpdateToDoStateContainer.Id;

            await Js.InvokeVoidAsync("removeToDo", id);
            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getToDos");

            NavigationManager.NavigateTo("/todo");

            // Navigating away from this page calls Dispose() which resets CreateUpdateToDoStateContainer,
            // so CreateUpdateToDoStateContainer.Id returns null. Therefore, the value has to be assigned
            // to a separate variable first.
            await HttpClient.DeleteAsync($"api/ToDos/{id}");
        }

        protected override async Task Save()
        {
            var toPersist = new ToDoModel
            {
                Id = ToDoId,
                Name = CreateUpdateToDoStateContainer.Name,
                For = CreateUpdateToDoStateContainer.For,
                ToDos = ToDoAdapter.ToInnerModel(CreateUpdateToDoStateContainer.ToDos),
                CreatedOn = CreateUpdateToDoStateContainer.CreatedOn,
            };
            await Js.InvokeVoidAsync("updateToDo", toPersist);

            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getToDos");
            NavigationManager.NavigateTo("/todo");

            var toUpdate = ToDoAdapter.ToUpdateDto(toPersist);
            await HttpClient.PutAsJsonAsync("api/ToDos", toUpdate);
        }

        public override void Dispose()
        {
            CreateUpdateToDoStateContainer.Reset();
        }
    }
}
