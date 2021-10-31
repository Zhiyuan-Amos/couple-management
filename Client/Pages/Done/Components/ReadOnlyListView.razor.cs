using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Couple.Client.Model.Image;
using Couple.Client.ViewModel.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Done.Components
{
    public partial class ReadOnlyListView
    {
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        private SortedDictionary<DateOnly, List<object>> DateToItems { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            DateToItems = await GetDateToItems();
        }

        private async Task<SortedDictionary<DateOnly, List<object>>> GetDateToItems()
        {
            var dateStringToCompletedElements = await Js.InvokeAsync<Dictionary<string, List<JsonElement>>>("getDone");
            var dateToCompletedItems = dateStringToCompletedElements
                .ToDictionary(kvp => DateOnly.ParseExact(kvp.Key, "dd/MM/yyyy"), kvp =>
                    kvp.Value
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

        private void EditImage(ImageModel selectedImage) => NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
    }
}
