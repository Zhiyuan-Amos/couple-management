using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Couple.Client.States.Image;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Image;

public partial class UpdateImage
{
    private ImageModel _imageModel;
    [EditorRequired] [Parameter] public Guid ImageId { get; set; }
    private CreateUpdateImageStateContainer CreateUpdateImageStateContainer { get; set; }

    [Inject] private DbContextProvider DbContextProvider { get; init; }
    [Inject] private HttpClient HttpClient { get; init; }
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }

    protected override void OnInitialized()
    {
        if (!DoneStateContainer.TryGetImage(ImageId, out _imageModel))
        {
            NavigationManager.NavigateTo("/done");
        }

        CreateUpdateImageStateContainer =
            new(_imageModel.TakenOn, _imageModel.IsFavourite, _imageModel.Data);
    }

    private async Task Delete()
    {
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        db.Images.Remove(_imageModel);
        await db.SaveChangesAsync();

        NavigationManager.NavigateTo("/done");

        await HttpClient.DeleteAsync($"api/Images/{ImageId}");
    }

    private async Task Save()
    {
        var toUpdate = new ImageModel(_imageModel.Id, _imageModel.TakenOn, _imageModel.Data, _imageModel.IsFavourite);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        db.Attach(toUpdate);

        toUpdate.TakenOn = CreateUpdateImageStateContainer.DateTime;
        toUpdate.Data = CreateUpdateImageStateContainer.Data;
        toUpdate.IsFavourite = CreateUpdateImageStateContainer.IsFavourite;
        db.Images.Update(toUpdate);
        await db.SaveChangesAsync();

        NavigationManager.NavigateTo("/done");

        var toUpdateDto = ImageAdapter.ToUpdateDto(toUpdate);
        await HttpClient.PutAsJsonAsync("api/Images", toUpdateDto);
    }
}
