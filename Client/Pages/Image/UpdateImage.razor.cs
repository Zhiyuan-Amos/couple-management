using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Image
{
    public partial class UpdateImage
    {
        [Parameter] public Guid ImageId { get; set; }
        private UpdateImageModel _image;

        [Inject] private HttpClient HttpClient { get; init; }
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        protected override async Task OnInitializedAsync()
        {
            _image = await Js.InvokeAsync<UpdateImageModel>("getImage", ImageId);
            if (_image == null)
            {
                NavigationManager.NavigateTo("/done");
            }
        }

        private async Task Delete()
        {
            await Js.InvokeVoidAsync("deleteImage", ImageId);
            NavigationManager.NavigateTo("/done");

            await HttpClient.DeleteAsync($"api/Images/{ImageId}");
        }

        private async Task Save()
        {
            await Js.InvokeVoidAsync("updateImage", _image);

            NavigationManager.NavigateTo("/done");

            var toUpdate = ImageAdapter.ToUpdateDto(_image);
            await HttpClient.PutAsJsonAsync("api/Images", toUpdate);
        }
    }
}
