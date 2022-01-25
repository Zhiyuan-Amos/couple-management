using Couple.Client.Model.Image;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer;

public class CreateImageCommand : ICommand
{
    private readonly IJSRuntime _js;
    private readonly ImageModel _model;

    public CreateImageCommand(IJSRuntime js, ImageModel model) => (_js, _model) = (js, model);

    public async Task Execute() => await _js.InvokeVoidAsync("createImage", _model);
}
