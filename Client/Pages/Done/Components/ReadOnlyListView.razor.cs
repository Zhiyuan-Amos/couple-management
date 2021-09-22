using Couple.Client.Adapters;
using Couple.Client.Model.Image;
using Couple.Client.Model.Issue;
using Couple.Client.States.Issue;
using Couple.Client.ViewModel.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Done.Components
{
    public partial class ReadOnlyListView
    {
        [Inject] private IssueStateContainer IssueStateContainer { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        private IReadOnlyDictionary<DateOnly, List<CompletedTaskViewModel>> DateToTasks =>
            IssueStateContainer.DateToCompletedTasks;

        private SortedDictionary<DateOnly, List<string>> DateToImages { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            IssueStateContainer.SetDateToCompletedTasks(await GetDateToCompletedTasks());
            DateToImages = await GetDateToImages();
        }

        private async Task<SortedDictionary<DateOnly, List<CompletedTaskViewModel>>> GetDateToCompletedTasks()
        {
            var dateStringToCompletedTasks = await Js.InvokeAsync<Dictionary<string, List<CompletedTaskViewModel>>>("getCompletedTasks");
            var dateToCompletedTasks = dateStringToCompletedTasks
                .ToDictionary(kvp => DateOnly.ParseExact(kvp.Key, "dd/MM/yyyy"), kvp => kvp.Value);
            return new(dateToCompletedTasks);
        }

        private async Task<SortedDictionary<DateOnly, List<string>>> GetDateToImages()
        {
            var dateToImageModels = await Js.InvokeAsync<Dictionary<string, List<ImageModel>>>("getImages");
            var dateToImages = dateToImageModels
                .ToDictionary(kvp => DateOnly.ParseExact(kvp.Key, "dd/MM/yyyy"),
                    kvp => kvp.Value.Select(image => Convert.ToBase64String(image.Data)).ToList());
            return new(dateToImages);
        }
    }
}
