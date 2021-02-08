using BlazorState;
using Couple.Client.Data;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public abstract class CreateUpdateToDoBase : BlazorStateComponent
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected LocalStore LocalStore { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected ToDoDataState ToDoDataState => GetState<ToDoDataState>();

        protected abstract Task Save();

        protected abstract Task ShowSelectionWindow();
        protected abstract Task Select(string category);
        protected abstract Task CancelSelection();

        protected abstract bool IsEnabled { get; }
    }
}
