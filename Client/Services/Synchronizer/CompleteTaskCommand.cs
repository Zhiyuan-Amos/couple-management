using Couple.Client.Model.Issue;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer;

public class CompleteTaskCommand : ICommand
{
    private readonly IJSRuntime _js;
    private readonly CreateCompletedTaskModel _model;

    public CompleteTaskCommand(IJSRuntime js, CreateCompletedTaskModel model) => (_js, _model) = (js, model);

    public async Task Execute() => await _js.InvokeVoidAsync("completeTask", _model);
}
