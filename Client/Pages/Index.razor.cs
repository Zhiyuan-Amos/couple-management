using System.Collections.Generic;
using System.Threading.Tasks;
using Couple.Client.Model.Image;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages;

public partial class Index
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IJSRuntime Js { get; init; }
    private List<ImageModel> FavouriteImages { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        FavouriteImages = await Js.InvokeAsync<List<ImageModel>>("getFavouriteImages");
    }

    private void EditImage(ImageModel selectedImage) => NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
}
