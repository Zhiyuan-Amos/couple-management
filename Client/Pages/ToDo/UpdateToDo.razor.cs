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
        private ToDoModel _currentToDoModel;

        protected override void OnInitialized()
        {
            if (!ToDoStateContainer.TryGetToDo(ToDoId, out _currentToDoModel))
            {
                NavigationManager.NavigateTo("/todo");
                return;
            }

            CreateUpdateToDoStateContainer.Initialize(_currentToDoModel.Name,
                _currentToDoModel.For,
                _currentToDoModel.ToDos);
        }

        protected async Task Delete()
        {
            await Js.InvokeVoidAsync("removeToDo", ToDoId);
            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getToDos");

            NavigationManager.NavigateTo("/todo");

            await HttpClient.DeleteAsync($"api/ToDos/{ToDoId}");
        }

        protected override async Task Save()
        {
            var toPersist = new ToDoModel
            {
                Id = ToDoId,
                Name = CreateUpdateToDoStateContainer.Name,
                For = CreateUpdateToDoStateContainer.For,
                ToDos = ToDoAdapter.ToInnerModel(CreateUpdateToDoStateContainer.ToDos),
                CreatedOn = _currentToDoModel.CreatedOn,
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
