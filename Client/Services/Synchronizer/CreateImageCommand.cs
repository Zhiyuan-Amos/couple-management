using System.Threading.Tasks;
using Couple.Client.Model.Image;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer
{
    public class CreateImageCommand : ICommand
    {
        private readonly IJSRuntime _js;
        private readonly CreateImageModel _model;
        public CreateImageCommand(IJSRuntime js, CreateImageModel model) => (_js, _model) = (js, model);

        public Task Execute() => _js.InvokeVoidAsync("createImage", _model).AsTask();
    }
}
