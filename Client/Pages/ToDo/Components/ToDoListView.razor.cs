using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class ToDoListView
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        [Parameter] public List<ToDoViewModel> ToDos { get; set; }

        private void EditToDo(ToDoViewModel selectedToDo) => NavigationManager.NavigateTo($"/todo/{selectedToDo.Id}");
    }
}
