using Couple.Client.Model.Issue;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer;

public class UpdateIssueCommand : ICommand
{
    private readonly IJSRuntime _js;
    private readonly IssueModel _model;

    public UpdateIssueCommand(IJSRuntime js, IssueModel model)
    {
        (_js, _model) = (js, model);
    }

    public Task Execute()
    {
        return _js.InvokeVoidAsync("updateIssue", _model).AsTask();
    }
}
