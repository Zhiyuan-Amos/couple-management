using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Settings;

public partial class Settings
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IJSRuntime Js { get; init; }
    private string ProgressMessage { get; set; } = "";

    private async Task OnImportSelected(InputFileChangeEventArgs e)
    {
        if (e.File.ContentType != "application/json")
        {
            ProgressMessage = "Invalid file";
            StateHasChanged();
            return;
        }

        try
        {
            ProgressMessage = "Valid file";
            StateHasChanged();
            await Js.InvokeVoidAsync("clearDatabase");
            ProgressMessage = "Database cleared";
            StateHasChanged();
            var json =
                await new StreamReader(e.File.OpenReadStream(512000000)).ReadToEndAsync();
            ProgressMessage = "Json read into memory";
            StateHasChanged();
            await Js.InvokeVoidAsync("importDatabase", json);
            ProgressMessage = "Database imported";
            StateHasChanged();
        }
        catch (Exception ex)
        {
            ProgressMessage = ex.Message;
            StateHasChanged();
        }
    }

    private async Task OnExportSelected() => await Js.InvokeAsync<object>("exportDatabaseAsFile", "couple.json");

    private async Task OnDeleteDatabaseSelected() => await Js.InvokeVoidAsync("deleteDatabase");

    private void OnUploadImageSelected() => NavigationManager.NavigateTo("/image/create");
}
