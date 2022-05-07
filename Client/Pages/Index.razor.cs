using Couple.Client.Model.Done;
using Couple.Client.Model.Image;
using Couple.Client.Services.Settings;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Couple.Client.Pages;

public partial class Index
{
    private static bool s_isDataLoaded;
    private static bool s_isDataSynchronized;
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;
    
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;
    [Inject] private Synchronizer Synchronizer { get; init; } = default!;
    [Inject] private IJSRuntime Js { get; init; } = default!;
    [Inject] private IOptions<AuthenticationOptions> AuthOptions { get; init; } = default!;

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var ao = AuthOptions.Value;
        // This seems necessary as https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-net-aad-b2c-considerations#acquire-a-token-to-apply-a-policy doesn't work
        Js.InvokeVoidAsync("login", ao.ClientId, ao.Authority, ao.KnownAuthority, ao.RedirectUri)
            .AsTask()
            .ContinueWith(_ => AuthenticationStateTask) // Ensures that background API calls are only done after authentication state has been set
            .ContinueWith(_ =>
            {
                if (!s_isDataSynchronized)
                {
                    s_isDataSynchronized = true;
                    return Synchronizer.SynchronizeAsync();
                }

                return Task.CompletedTask;
            });
    }

    private void EditImage(IReadOnlyImageModel selectedImage) =>
        NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
}
