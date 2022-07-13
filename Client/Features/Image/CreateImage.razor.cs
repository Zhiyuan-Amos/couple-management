using System.Net.Http.Json;
using Couple.Client.Features.Done.States;
using Couple.Client.Features.Image.Adapters;
using Couple.Client.Features.Image.Models;
using Couple.Client.Features.Image.States;
using Couple.Client.Features.Synchronizer;
using Couple.Shared;
using Couple.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Couple.Client.Features.Image;

public partial class CreateImage
{
    private bool _isSaveClicked;
    private CreateImageStateContainer CreateImageStateContainer { get; set; } = default!;

    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;

    private bool IsSaveEnabled => CreateImageStateContainer.Data.Any() && !_isSaveClicked;

    protected override void OnInitialized() => CreateImageStateContainer = new();

    private async Task Save()
    {
        _isSaveClicked = true;

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
        foreach (var file in e.GetMultipleFiles(100))
        {
            var extension = Path.GetExtension(file.Name);
            if (!ImageExtensions.IsImage(extension))
            {
                Console.Error.WriteLine("Invalid file type");
                continue;
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
                continue;
            }

            CreateImageStateContainer.Data.Add(ms.ToArray());
        }
    }
}
