using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer;

public class DeleteIssueCommand : ICommand
{
    private readonly IJSRuntime _js;
    private readonly Guid _guid;
    public DeleteIssueCommand(IJSRuntime js, Guid guid) => (_js, _guid) = (js, guid);

    public Task Execute() => _js.InvokeVoidAsync("deleteIssue", _guid).AsTask();
}
