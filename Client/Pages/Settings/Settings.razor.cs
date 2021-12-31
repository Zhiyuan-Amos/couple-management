using System.Dynamic;
using System.Text.Json;
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
        if (e.File.ContentType != "text/plain")
        {
            return;
        }

        await Js.InvokeVoidAsync("clearDatabase");

        using var reader = new StreamReader(e.File.OpenReadStream(512000000));
        string? type;
        while ((type = await reader.ReadLineAsync()) != null)
        {
            switch (type)
            {
                case "done":
                    {
                        ProgressMessage = "Importing Done";
                        StateHasChanged();
                        var line = await reader.ReadLineAsync();
                        while (line != "---")
                        {
                            var date = line;
                            var item = await reader.ReadLineAsync();
                            var count = 0;
                            while (item != "")
                            {
                                var toPersist = JsonSerializer.Deserialize<ExpandoObject>(item);
                                await Js.InvokeVoidAsync("importDone", toPersist, date);
                                item = await reader.ReadLineAsync();

                                ProgressMessage = count.ToString();
                                StateHasChanged();
                                count++;
                            }

                            line = await reader.ReadLineAsync();
                        }

                        break;
                    }
                case "event":
                    {
                        var line = await reader.ReadLineAsync();
                        while (line != "---")
                        {
                            line = await reader.ReadLineAsync();
                        }

                        break;
                    }
                case "image":
                    {
                        ProgressMessage = "Importing Image";
                        StateHasChanged();
                        var line = await reader.ReadLineAsync();
                        var count = 0;
                        while (line != "---")
                        {
                            var toPersist = JsonSerializer.Deserialize<ExpandoObject>(line);
                            await Js.InvokeVoidAsync("importImage", toPersist);
                            line = await reader.ReadLineAsync();

                            ProgressMessage = count.ToString();
                            StateHasChanged();
                            count++;
                        }

                        break;
                    }
                case "issue":
                    {
                        ProgressMessage = "Importing Issue";
                        StateHasChanged();
                        var line = await reader.ReadLineAsync();
                        var count = 0;
                        while (line != "---")
                        {
                            var toPersist = JsonSerializer.Deserialize<ExpandoObject>(line);
                            await Js.InvokeVoidAsync("importIssue", toPersist);
                            line = await reader.ReadLineAsync();

                            ProgressMessage = count.ToString();
                            StateHasChanged();
                            count++;
                        }

                        break;
                    }
            }
        }

        ProgressMessage = "Database imported";
        StateHasChanged();
    }

    private async Task OnExportSelected() => await Js.InvokeAsync<object>("exportDatabaseAsFile", "couple.txt");

    private async Task OnDeleteDatabaseSelected() => await Js.InvokeVoidAsync("deleteDatabase");

    private void OnUploadImageSelected() => NavigationManager.NavigateTo("/image/create");
}
