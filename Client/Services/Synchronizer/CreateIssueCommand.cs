using Couple.Client.Model.Issue;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer;

public class CreateIssueCommand : ICommand
{
    private readonly IJSRuntime _js;
    private readonly IssueModel _model;

    public CreateIssueCommand(IJSRuntime js, IssueModel model)
    {
        (_js, _model) = (js, model);
    }

    public Task Execute()
    {
        return _js.InvokeVoidAsync("createIssue", _model).AsTask();
    }
}
