using Couple.Client.ViewModel.ToDo;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Couple.Client.Pages.ToDo.Components
{
    public partial class ToDoListView
    {
        [Inject]
        protected NavigationManager NavigationManager { get; init; }

        [Parameter]
        public List<ToDoViewModel> ToDos { get; set; }

        protected void EditToDo(ToDoViewModel selectedToDo) => NavigationManager.NavigateTo($"/todo/{selectedToDo.Id}");
    }
}
