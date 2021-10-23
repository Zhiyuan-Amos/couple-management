using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Shared;
using Couple.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Settings
{
    public partial class Settings
    {
        [Inject] private HttpClient HttpClient { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        private async Task OnImportSelected(InputFileChangeEventArgs e)
        {
            if (e.File.ContentType != "application/json")
            {
                return;
            }

            await Js.InvokeVoidAsync("clearDatabase");
            var json =
                await new StreamReader(e.File.OpenReadStream(512000000)).ReadToEndAsync();
            await Js.InvokeVoidAsync("importDatabase", json);
        }

        private async Task OnExportSelected()
        {
            var json = await Js.InvokeAsync<string>("exportDatabase");
            var bytes = Encoding.UTF8.GetBytes(json);
            const string fileName = "couple.json";

            if (Js is IJSUnmarshalledRuntime webAssemblyJsRuntime)
            {
                webAssemblyJsRuntime.InvokeUnmarshalled<string, string, byte[], bool>("saveAsFileFast",
                    fileName, "application/octet-stream", bytes);
            }
            else
            {
                await Js.InvokeAsync<object>(
                    "saveAsFile",
                    fileName,
                    Convert.ToBase64String(bytes));
            }
        }

        private async Task OnDeleteDatabaseSelected()
        {
            await Js.InvokeVoidAsync("deleteDatabase");
        }

        private async Task OnUploadImageSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            var extn = Path.GetExtension(file.Name);
            if (!ImageExtensions.IsImage(extn))
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

            var toPersist = new CreateImageModel(Guid.NewGuid(), DateTime.Now, ms.ToArray());
            await Js.InvokeVoidAsync("saveImage", toPersist);

            var toCreate = ImageAdapter.ToDto(toPersist);
            await HttpClient.PostAsJsonAsync("api/Images", toCreate);
        }
    }
}
