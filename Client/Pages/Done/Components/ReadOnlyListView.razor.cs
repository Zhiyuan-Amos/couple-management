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

namespace Couple.Client.Pages.Done.Components
{
    public partial class ReadOnlyListView : IDisposable
    {
        [Inject] private ToDoStateContainer ToDoStateContainer { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        private List<CompletedToDoViewModel> ToDos
        {
            get
            {
                var orderedToDos = ToDoStateContainer.CompletedToDos
                    .OrderBy(toDo => toDo.CompletedOn)
                    .ToList();
                return ToDoAdapter.ToCompletedViewModel(orderedToDos);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ToDoStateContainer.OnChange += StateHasChanged;
            ToDoStateContainer.CompletedToDos = await Js.InvokeAsync<List<CompletedToDoModel>>("getCompletedToDos");
        }

        public void Dispose() => ToDoStateContainer.OnChange -= StateHasChanged;
    }
}
