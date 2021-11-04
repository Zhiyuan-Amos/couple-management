using System.Threading.Tasks;
using Couple.Client.Model.Image;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer
{
    public class UpdateImageCommand : ICommand
    {
        private readonly IJSRuntime _js;
        private readonly ImageModel _model;
        public UpdateImageCommand(IJSRuntime js, ImageModel model) => (_js, _model) = (js, model);

        public Task Execute() => _js.InvokeVoidAsync("updateImage", _model).AsTask();
    }
}
