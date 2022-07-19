using System.Net.Http.Json;
using Couple.Client.Features.Done.States;
using Couple.Client.Features.Image.Adapters;
using Couple.Client.Features.Image.Models;
using Couple.Client.Features.Image.States;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Image;

public partial class UpdateImage
{
    private IReadOnlyImageModel _imageModel = default!;
    [EditorRequired][Parameter] public Guid ImageId { get; set; }
    private UpdateImageStateContainer UpdateImageStateContainer { get; set; } = default!;

    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;

    protected override void OnInitialized()
    {
        if (!DoneStateContainer.TryGetImage(ImageId, out _imageModel!))
        {
            NavigationManager.NavigateTo("/done");
        }

        UpdateImageStateContainer =
            new(_imageModel.TakenOn, _imageModel.IsFavourite, _imageModel.Data);
    }

    private async Task Delete()
    {
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        var toDelete = new ImageModel(_imageModel.Id, _imageModel.TakenOn, _imageModel.Data, _imageModel.IsFavourite);
        db.Images.Remove(toDelete);
        await db.SaveChangesAsync();

        DoneStateContainer.DeleteImage(toDelete.Id);

        NavigationManager.NavigateTo("/done");

        await HttpClient.DeleteAsync($"api/Image/{ImageId}");
    }

    private async Task Save()
    {
        var toUpdate = new ImageModel(_imageModel.Id, _imageModel.TakenOn, _imageModel.Data, _imageModel.IsFavourite);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        db.Attach(toUpdate);

        toUpdate.TakenOn = UpdateImageStateContainer.DateTime;
        toUpdate.Data = UpdateImageStateContainer.Data!;
        toUpdate.IsFavourite = UpdateImageStateContainer.IsFavourite;

        await db.SaveChangesAsync();

        DoneStateContainer.UpdateImage(toUpdate);

        NavigationManager.NavigateTo("/done");

        var toUpdateDto = ImageAdapter.ToUpdateDto(toUpdate);
        await HttpClient.PutAsJsonAsync("api/Image", toUpdateDto);
    }
}
