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
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;

    private List<IReadOnlyImageModel> FavouriteImages =>
        DoneStateContainer.FavouriteImages
            .OrderByDescending(image => image.TakenOn)
            .ThenBy(image => image.Order)
            .ToList();

    protected override async Task OnInitializedAsync()
    {
        DoneStateContainer.OnChange += StateHasChanged;

        if (s_isDataLoaded)
        {
            return;
        }

        if (DoneStateContainer.GetDateToItems().Count != 0)
        {
            s_isDataLoaded = true;
            return;
        }

        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        var favouriteImages = await db.Images
            .Where(i => i.IsFavourite)
            .ToListAsync();

        if (favouriteImages.Count > 0)
        {
            var toSet = favouriteImages.Cast<IDone>().ToList();
            DoneStateContainer.SetItems(toSet);
        }

        s_isDataLoaded = true;
    }

    private void EditImage(IReadOnlyImageModel selectedImage) =>
        NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
}
