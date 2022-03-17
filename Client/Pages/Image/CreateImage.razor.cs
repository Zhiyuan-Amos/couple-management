using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Couple.Client.States.Image;
using Couple.Shared;
using Couple.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Couple.Client.Pages.Image;

public partial class CreateImage
{
    private CreateImageStateContainer CreateImageStateContainer { get; set; } = default!;

    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;

    private bool IsSaveEnabled => CreateImageStateContainer.Data.Any();

    protected override void OnInitialized() => CreateImageStateContainer = new();

    private async Task Save()
    {
        foreach (var data in CreateImageStateContainer.Data)
        {
            var toPersist = new ImageModel(CreateImageStateContainer.DateTime,
                data,
                CreateImageStateContainer.IsFavourite);

            await using var db = await DbContextProvider.GetPreparedDbContextAsync();
            db.Images.Add(toPersist);
            await db.SaveChangesAsync();

            DoneStateContainer.AddImage(toPersist);

            var toCreate = ImageAdapter.ToCreateDto(toPersist);
            await HttpClient.PostAsJsonAsync("api/Images", toCreate);
        }

        NavigationManager.NavigateTo("/settings");
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

        CreateImageStateContainer.Data.Add(ms.ToArray());
    }
}
