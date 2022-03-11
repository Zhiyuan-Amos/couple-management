using Couple.Client.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Settings;

public partial class Settings
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private HttpClient HttpClient { get; init; }
    [Inject] private IJSRuntime Js { get; init; }

    private async Task OnImportSelected(InputFileChangeEventArgs e)
    {
        var stream = e.File.OpenReadStream(512000000);
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        await Js.InvokeVoidAsync("importDatabase", ms.ToArray());
        await HttpClient.DeleteAsync("api/Changes/all");
    }

    private async Task OnExportSelected() => await Js.InvokeVoidAsync("exportDatabase", Constants.DatabaseFileName);

    private async Task OnDeleteDatabaseSelected() => await Js.InvokeVoidAsync("deleteDatabase");

    private void OnUploadImageSelected() => NavigationManager.NavigateTo("/image/create");
}
