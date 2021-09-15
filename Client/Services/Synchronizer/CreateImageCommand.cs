using Couple.Client.Model.Image;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Couple.Client.Services.Synchronizer
{
    public class CreateImageCommand : ICommand
    {
        private readonly IJSRuntime _js;
        private readonly ImageModel _model;
        public CreateImageCommand(IJSRuntime js, ImageModel model) => (_js, _model) = (js, model);

        public Task Execute() => _js.InvokeVoidAsync("saveImage", _model).AsTask();
    }
}
