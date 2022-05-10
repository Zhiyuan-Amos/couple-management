using Couple.Client.Services.Settings;
using Couple.Client.Utility;

namespace Couple.Client.Pages.Settings;

public class Settings
{
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private IJSRuntime Js { get; init; } = default!;
    [Inject] private IOptions<AuthenticationOptions> AuthOptions { get; init; } = default!;

    private async Task OnImportSelected(InputFileChangeEventArgs e)
    {
        var stream = e.File.OpenReadStream(512000000);
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        await Js.InvokeVoidAsync("importDatabase", ms.ToArray());
        await HttpClient.DeleteAsync("api/Changes/all");
    }

    private async Task OnExportSelected()
    {
        return await Js.InvokeVoidAsync("exportDatabase", Constants.DatabaseFileName);
    }

    private async Task OnDeleteDatabaseSelected()
    {
        return await Js.InvokeVoidAsync("deleteDatabase");
    }

    private void OnUploadImageSelected()
    {
        NavigationManager.NavigateTo("/image/create");
    }

    private async Task OnLogoutSelected()
    {
        // This seems necessary as https://stackoverflow.com/questions/64833025/blazor-webassembly-app-authentication-logout-results-in-there-was-an-error-try doesn't work
        var ao = AuthOptions.Value;
        await Js.InvokeVoidAsync("logout", ao.ClientId, ao.Authority, ao.KnownAuthority, ao.PostLogoutRedirectUri);
    }
}