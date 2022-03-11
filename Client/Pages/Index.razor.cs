using Couple.Client.Model.Done;
using Couple.Client.Model.Image;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Pages;

public partial class Index
{
    private static bool s_isDataLoaded;
    [Inject] private DbContextProvider DbContextProvider { get; init; }
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }
    private List<ImageModel> FavouriteImages { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (s_isDataLoaded)
        {
            return;
        }

        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        FavouriteImages = await db.Images
            .Where(i => i.IsFavourite)
            .ToListAsync();

        if (FavouriteImages.Count > 0 && DoneStateContainer.GetDateToItems().Count == 0)
        {
            var toSet = FavouriteImages.Cast<IDone>().ToList();
            DoneStateContainer.SetItems(toSet);
        }

        s_isDataLoaded = true;
    }

    private void EditImage(ImageModel selectedImage) => NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
}
