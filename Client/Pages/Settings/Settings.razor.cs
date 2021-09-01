using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Settings
{
    public partial class Settings
    {
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
    }
}
