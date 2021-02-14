using Couple.Client.Data;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public abstract class CreateUpdateToDoBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected LocalStore LocalStore { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ToDoStateContainer ToDoStateContainer { get; set; }

        [Inject]
        protected SelectedCategoryStateContainer SelectedCategoryStateContainer { get; set; }

        protected abstract Task Save();

        protected abstract Task ShowSelectionWindow();
        protected abstract Task Select(string category);
        protected abstract Task CancelSelection();

        protected abstract bool IsEnabled { get; }
    }
}
