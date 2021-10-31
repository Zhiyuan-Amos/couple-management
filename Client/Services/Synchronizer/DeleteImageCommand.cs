using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer
{
    public class DeleteImageCommand : ICommand
    {
        private readonly IJSRuntime _js;
        private readonly Guid _guid;
        public DeleteImageCommand(IJSRuntime js, Guid guid) => (_js, _guid) = (js, guid);

        public Task Execute() => _js.InvokeVoidAsync("deleteImage", _guid).AsTask();
    }
}
