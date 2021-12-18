using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Client.States.Done;
using Couple.Client.States.Image;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Image;

public partial class UpdateImage
{
    private ImageModel _imageModel;
    [EditorRequired] [Parameter] public Guid ImageId { get; set; }
    private CreateUpdateImageStateContainer CreateUpdateImageStateContainer { get; set; }

    [Inject] private HttpClient HttpClient { get; init; }
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IJSRuntime Js { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }

    protected override void OnInitialized()
    {
        if (!DoneStateContainer.TryGetImage(ImageId, out _imageModel))
        {
            NavigationManager.NavigateTo("/done");
        }

        CreateUpdateImageStateContainer =
            new(_imageModel.IsFavourite, _imageModel.Data);
    }

    private async Task Delete()
    {
        await Js.InvokeVoidAsync("deleteImage", ImageId);
        NavigationManager.NavigateTo("/done");

        await HttpClient.DeleteAsync($"api/Images/{ImageId}");
    }

    private async Task Save()
    {
        var toPersist = new ImageModel(_imageModel.Id, _imageModel.TakenOn,
            CreateUpdateImageStateContainer.Data, CreateUpdateImageStateContainer.IsFavourite);
        await Js.InvokeVoidAsync("updateImage", toPersist);

        NavigationManager.NavigateTo("/done");

        var toUpdate = ImageAdapter.ToUpdateDto(toPersist);
        await HttpClient.PutAsJsonAsync("api/Images", toUpdate);
    }
}
