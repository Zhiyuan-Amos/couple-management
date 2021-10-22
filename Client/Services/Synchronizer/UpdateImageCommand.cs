using Couple.Client.Model.Image;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Couple.Client.Services.Synchronizer
{
    public class UpdateImageCommand : ICommand
    {
        private readonly IJSRuntime _js;
        private readonly UpdateImageModel _model;
        public UpdateImageCommand(IJSRuntime js, UpdateImageModel model) => (_js, _model) = (js, model);

        public Task Execute() => _js.InvokeVoidAsync("updateImage", _model).AsTask();
    }
}
