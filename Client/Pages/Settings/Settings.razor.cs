using Couple.Client.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Settings;

public partial class Settings
{
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private SignOutSessionStateManager SignOutManager { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private IJSRuntime Js { get; init; } = default!;

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

    private async Task OnLogoutSelected()
    {
        await SignOutManager.SetSignOutState();
        NavigationManager.NavigateTo("/authentication/logout");
    }
}
