using Couple.Client.Model.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class ToDoListView
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public List<ToDoViewModel> ToDos { get; set; }

        private void EditToDo(ToDoViewModel selectedToDo) => NavigationManager.NavigateTo($"/todo/{selectedToDo.Id}");

        private string GetIcon(For @for) => @for switch
        {
            For.Him => @"content: url(""icons/male.svg"")",
            For.Her => @"content: url(""icons/female.svg"")",
            For.Us => @"content: url(""icons/us.svg"")",
            _ => throw new ArgumentOutOfRangeException(nameof(@for), @for, null)
        };

        private async Task OnCheckboxToggle(Guid id, ToDoInnerViewModel toDo)
        {
            toDo.IsCompleted = !toDo.IsCompleted;
            var toUpdate = ToDos.Single(x => x.Id == id);
            await Js.InvokeVoidAsync("updateToDo", toUpdate);
        }
    }
}
