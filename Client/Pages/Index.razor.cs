using Couple.Client.Model.Image;
using Couple.Client.States.Done;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages;

public partial class Index
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IJSRuntime Js { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }
    private List<ImageModel> FavouriteImages { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        FavouriteImages = await Js.InvokeAsync<List<ImageModel>>("getFavouriteImages");
        if (FavouriteImages.Count > 0 && DoneStateContainer.GetDateToItems().Count == 0)
        {
            var toStore = FavouriteImages
                .GroupBy(image => DateOnly.FromDateTime(image.TakenOn))
                .ToDictionary(grouping => grouping.Key,
                    grouping => (IReadOnlyList<object>)grouping.Select(image => (object)image).ToList());
            DoneStateContainer.SetDateToItems(toStore);
        }
    }

    private void EditImage(ImageModel selectedImage)
    {
        NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
    }
}
