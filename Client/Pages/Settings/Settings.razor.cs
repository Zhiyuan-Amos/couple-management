using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Settings
{
    public partial class Settings
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
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

        private void OnUploadImageSelected()
        {
            NavigationManager.NavigateTo("/image/create");
        }
    }
}
