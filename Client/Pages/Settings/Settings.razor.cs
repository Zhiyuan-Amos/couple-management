using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Shared.Model.Image;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
                await new StreamReader(e.File.OpenReadStream()).ReadToEndAsync();
            await Js.InvokeVoidAsync("importDatabase", json);
        }

        private async Task OnExportSelected()
        {
            var json = await Js.InvokeAsync<string>("exportDatabase");
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            await Js.InvokeAsync<object>(
                "saveAsFile",
                "couple.json",
                Convert.ToBase64String(bytes));
        }

        private async Task OnDeleteDatabaseSelected()
        {
            await Js.InvokeVoidAsync("deleteDatabase");
        }

        private async Task OnUploadImageSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var resizedFile = await file.RequestImageFileAsync(file.ContentType, 512, 1024);

            await using var ms = new MemoryStream();
            await resizedFile.OpenReadStream(5120000).CopyToAsync(ms);

            var toPersist = new ImageModel(Guid.NewGuid(), DateTime.Now, ms.ToArray());
            //await Js.InvokeVoidAsync("saveImage", toPersist);

            var toSend = ImageAdapter.ToDto(toPersist);
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(resizedFile.OpenReadStream(5120000)), "\"image\"", resizedFile.Name);
            content.Add(JsonContent.Create(toSend), "\"data\"");

            await HttpClient.PostAsync("api/Images", content);
        }
    }
}
