using Couple.Client.Model.Issue;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Couple.Client.Services.Synchronizer
{
    public class CompleteTaskCommand : ICommand
    {
        private readonly IJSRuntime _js;
        private readonly CompletedTaskModel _model;
        public CompleteTaskCommand(IJSRuntime js, CompletedTaskModel model) => (_js, _model) = (js, model);

        public Task Execute() => _js.InvokeVoidAsync("completeTask", _model).AsTask();
    }
}
