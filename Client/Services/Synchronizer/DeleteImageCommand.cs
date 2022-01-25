using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer;

public class DeleteImageCommand : ICommand
{
    private readonly Guid _guid;
    private readonly IJSRuntime _js;

    public DeleteImageCommand(IJSRuntime js, Guid guid) => (_js, _guid) = (js, guid);

    public async Task Execute() => await _js.InvokeVoidAsync("deleteImage", _guid);
}
