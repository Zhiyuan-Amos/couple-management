using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

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
            await Js.InvokeAsync<object>("saveAsFile", "couple.json");
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
