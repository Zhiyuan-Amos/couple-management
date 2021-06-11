using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public partial class ToDo
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        [Inject] private ToDoStateContainer ToDoStateContainer { get; init; }

        private List<ToDoViewModel> ToDos => ToDoAdapter.ToViewModel(ToDoStateContainer.ToDos);

        protected override async Task OnInitializedAsync()
        {
            ToDoStateContainer.OnChange += StateHasChanged;
            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getAllToDos");

            if (!ToDoStateContainer.TryGetToDo(Guid.Empty, out _))
            {
                var toPersist = new ToDoModel
                {
                    Id = Guid.Empty,
                    Name = "Name",
                    For = For.Us,
                    ToDos = new() {new() {Content = "ToDo", IsCompleted = false,}},
                    CreatedOn = DateTime.Now,
                };

                await Js.InvokeVoidAsync("addToDo", toPersist);
                ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getAllToDos");
            }
        }

        public void Dispose() => ToDoStateContainer.OnChange -= StateHasChanged;

        private void AddToDo() => NavigationManager.NavigateTo($"/todo/create");
    }
}
