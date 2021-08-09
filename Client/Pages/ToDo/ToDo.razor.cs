using Couple.Client.Adapters;
using Couple.Client.Model.ToDo;
using Couple.Client.States.ToDo;
using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public partial class ToDo : IDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        [Inject] private ToDoStateContainer ToDoStateContainer { get; init; }

        private List<ToDoViewModel> ToDos
        {
            get
            {
                var orderedToDos = ToDoStateContainer.ToDos
                    .OrderByDescending(toDo => toDo.CreatedOn)
                    .ToList();
                return ToDoAdapter.ToViewModel(orderedToDos);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ToDoStateContainer.OnChange += StateHasChanged;
            ToDoStateContainer.ToDos = await Js.InvokeAsync<List<ToDoModel>>("getAllToDos");
        }

        public void Dispose() => ToDoStateContainer.OnChange -= StateHasChanged;

        private void AddToDo() => NavigationManager.NavigateTo($"/todo/create");
    }
}
