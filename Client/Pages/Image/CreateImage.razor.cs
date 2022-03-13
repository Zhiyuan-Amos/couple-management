using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Image;
using Couple.Shared;
using Couple.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Couple.Client.Pages.Image;

public partial class CreateImage
{
    private CreateUpdateImageStateContainer CreateUpdateImageStateContainer { get; set; } = default!;

    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;

    private bool IsSaveEnabled => CreateUpdateImageStateContainer.Data != null;

    protected override void OnInitialized() => CreateUpdateImageStateContainer = new();

    private async Task Save()
    {
        var toPersist = new ImageModel(CreateUpdateImageStateContainer.DateTime,
            CreateUpdateImageStateContainer.Data!,
            CreateUpdateImageStateContainer.IsFavourite);

        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        db.Images.Add(toPersist);
        await db.SaveChangesAsync();

        NavigationManager.NavigateTo("/settings");

        var toCreate = ImageAdapter.ToCreateDto(toPersist);
        await HttpClient.PostAsJsonAsync("api/Images", toCreate);
    }

    private async Task OnUploadImageSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;

        var extension = Path.GetExtension(file.Name);
        if (!ImageExtensions.IsImage(extension))
        {
            Console.Error.WriteLine("Invalid file type");
            return;
        }

        // The above check is necessary as this method fails silently (unsure why surrounding it with try-catch
        // block does not trigger the code in the catch block) if a non-image file is uploaded.
        var resizedFile = await file.RequestImageFileAsync(file.ContentType, 800, 800);

        // `resizedFile` is untrusted, so validate against `resizedFile` as well.
        // See `file.RequestImageFileAsync`'s documentation.
        await using var ms = new MemoryStream();
        await resizedFile.OpenReadStream(Constants.MaxFileSize).CopyToAsync(ms);
        ms.Position = 0;

        if (!ImageExtensions.IsImage(ms))
        {
            Console.Error.WriteLine("Something went wrong");
            return;
        }

        CreateUpdateImageStateContainer.Data = ms.ToArray();
    }
}
