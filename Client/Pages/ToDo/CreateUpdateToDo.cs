using AutoMapper;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public abstract class CreateUpdateToDoBase : ComponentBase
    {
        [Inject] protected HttpClient HttpClient { get; init; }

        [Inject] protected NavigationManager NavigationManager { get; init; }

        [Inject] protected ToDoStateContainer ToDoStateContainer { get; init; }

        [Inject] protected SelectedCategoryStateContainer SelectedCategoryStateContainer { get; init; }

        [Inject] protected IMapper Mapper { get; init; }

        [Inject] protected IJSRuntime Js { get; init; }

        protected abstract Task Save();

        protected abstract Task ShowSelectionWindow();
        protected abstract Task Select(string category);
        protected abstract Task CancelSelection();

        protected abstract bool IsEnabled { get; }
    }
}
