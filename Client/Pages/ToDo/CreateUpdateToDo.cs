using AutoMapper;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Couple.Client.Pages.ToDo
{
    public abstract class CreateUpdateToDoBase : ComponentBase, IDisposable
    {
        [Inject] protected HttpClient HttpClient { get; init; }

        [Inject] protected NavigationManager NavigationManager { get; init; }

        [Inject] protected ToDoStateContainer ToDoStateContainer { get; init; }
        [Inject] protected CreateUpdateToDoStateContainer CreateUpdateToDoStateContainer { get; init; }

        [Inject] protected IMapper Mapper { get; init; }

        [Inject] protected IJSRuntime Js { get; init; }

        protected abstract Task Save();
        public abstract void Dispose();
    }
}
