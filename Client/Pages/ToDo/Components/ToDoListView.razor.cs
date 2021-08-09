using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class ToDoListView
    {
        [Inject] protected ToDoStateContainer ToDoStateContainer { get; init; }
        [Inject] protected HttpClient HttpClient { get; init; }
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public List<ToDoViewModel> ToDos { get; set; }

        private void EditToDo(ToDoViewModel selectedToDo) => NavigationManager.NavigateTo($"/todo/{selectedToDo.Id}");

        private async Task OnCheckboxToggle(Guid id, ToDoInnerViewModel toDo)
        {
            toDo.IsCompleted = !toDo.IsCompleted;
            var viewModel = ToDos.Single(x => x.Id == id);

            var isCompleted = viewModel.ToDos.All(innerToDo => innerToDo.IsCompleted);
            if (isCompleted)
            {
                var toPersist = ToDoAdapter.ToCompletedModel(viewModel, DateTime.Now);
                await Js.InvokeVoidAsync("completeToDo", toPersist);
                ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getToDos");

                var toUpdate = ToDoAdapter.ToCompleteDto(toPersist);
                await HttpClient.PutAsJsonAsync("api/ToDos/Complete", toUpdate);
            }
            else
            {
                var toPersist = ToDoAdapter.ToModel(viewModel);
                await Js.InvokeVoidAsync("updateToDo", toPersist);
                ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getToDos");

                var toUpdate = ToDoAdapter.ToUpdateDto(toPersist);
                await HttpClient.PutAsJsonAsync("api/ToDos", toUpdate);
            }
        }
    }
}
