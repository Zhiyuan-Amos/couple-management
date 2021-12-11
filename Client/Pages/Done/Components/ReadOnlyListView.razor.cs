using Couple.Client.Model.Image;
using Couple.Client.States.Done;
using Couple.Client.ViewModel.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Couple.Client.Pages.Done.Components;

public partial class ReadOnlyListView : IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IJSRuntime Js { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }

    public void Dispose()
    {
        DoneStateContainer.OnChange -= StateHasChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        DoneStateContainer.OnChange += StateHasChanged;
        DoneStateContainer.SetDateToItems(await GetDateToItems());
    }

    private async Task<SortedDictionary<DateOnly, IReadOnlyList<object>>> GetDateToItems()
    {
        var dateStringToCompletedElements = await Js.InvokeAsync<Dictionary<string, List<JsonElement>>>("getDone");
        var dateToCompletedItems = dateStringToCompletedElements
            .ToDictionary(kvp => DateOnly.ParseExact(kvp.Key, "dd/MM/yyyy"), kvp =>
                (IReadOnlyList<object>)kvp.Value
                    .Select(jsonElement =>
                        jsonElement.GetProperty("discriminator").GetString() switch
                        {
                            "CompletedTask" => (object)jsonElement.Deserialize<CompletedTaskViewModel>(
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                            "Image" => (object)jsonElement.Deserialize<ImageModel>(
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                            _ => throw new ArgumentException()
                        }
                    )
                    .ToList()
            );
        return new(dateToCompletedItems);
    }

    private void EditImage(ImageModel selectedImage)
    {
        NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
    }
}
