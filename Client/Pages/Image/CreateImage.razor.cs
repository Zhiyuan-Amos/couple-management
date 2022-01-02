using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Client.States.Image;
using Couple.Shared;
using Couple.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Image;

public partial class CreateImage
{
    private CreateUpdateImageStateContainer? CreateUpdateImageStateContainer { get; set; }

    [Inject] private HttpClient? HttpClient { get; init; }
    [Inject] private NavigationManager? NavigationManager { get; init; }
    [Inject] private IJSRuntime? Js { get; init; }

    private bool IsSaveEnabled
    {
        get
        {
            try
            {
                CreateUpdateImageStateContainer.GetDate();
            }
            catch (Exception e) when (e is ArgumentOutOfRangeException or FormatException)
            {
                return false;
            }

            return CreateUpdateImageStateContainer?.Data != null;
        }
    }

    protected override void OnInitialized() => CreateUpdateImageStateContainer = new();

    private async Task Save()
    {
        var date = CreateUpdateImageStateContainer.GetDate();
        var toPersist = new ImageModel(Guid.NewGuid(),
            new(date.Year, date.Month, date.Day),
            CreateUpdateImageStateContainer.Data, CreateUpdateImageStateContainer.IsFavourite);
        await Js.InvokeVoidAsync("createImage", toPersist);

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
