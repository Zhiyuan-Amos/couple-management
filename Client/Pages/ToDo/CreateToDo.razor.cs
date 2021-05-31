using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Couple.Shared.Model;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public class CreateToDoBase : CreateUpdateToDoBase
    {
        protected override void OnInitialized()
        {
            CreateUpdateToDoStateContainer.Initialize("",
                For.Him,
                new List<ToDoInnerModel>
                {
                    new()
                    {
                        Content = "",
                        IsCompleted = false,
                    }
                });
        }

        protected override async Task Save()
        {
            var id = Guid.NewGuid();
            var toPersist = new ToDoModel
            {
                Id = id,
                Name = CreateUpdateToDoStateContainer.Name,
                For = CreateUpdateToDoStateContainer.For,
                ToDos = ToDoAdapter.ToInnerModel(CreateUpdateToDoStateContainer.ToDos),
                CreatedOn = DateTime.Now,
            };
            await Js.InvokeVoidAsync("addToDo", toPersist);

            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getAllToDos");
            NavigationManager.NavigateTo("/todo");

            var toCreate = ToDoAdapter.ToCreateDto(toPersist);
            await HttpClient.PostAsJsonAsync($"api/ToDos", toCreate);
        }

        public override void Dispose()
        {
            CreateUpdateToDoStateContainer.Reset();
        }
    }
}
