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

        private SortedDictionary<DateOnly, List<CompletedTaskViewModel>> DateToTasks
        {
            get
            {
                var toReturn = IssueStateContainer.CompletedTasks
                    .GroupBy(task => DateOnly.FromDateTime(task.CreatedOn.Date))
                    .ToDictionary(dateToTasks => dateToTasks.Key,
                        dateToTasks =>
                        {
                            var issueToTasksForOneDate = dateToTasks
                                .GroupBy(task => task.IssueId)
                                .ToDictionary(issueToTasks => issueToTasks.Key,
                                    issueToTasks => issueToTasks
                                        .OrderByDescending(issue => issue.CreatedOn)
                                        .ToList());

                            return issueToTasksForOneDate.Values
                                .Select(tasks => new CompletedTaskViewModel(tasks[0].For,
                                    tasks.Select(task => task.Content).ToList(),
                                    tasks[0].IssueTitle,
                                    tasks[0].CreatedOn))
                                .OrderByDescending(task => task.CreatedOn)
                                .ToList();
                        });
                return new(toReturn);
            }
        }

        private List<string> Images { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            IssueStateContainer.CompletedTasks = await Js.InvokeAsync<List<CompletedTaskModel>>("getCompletedTasks");
            Images = await GetImages();
        }

        private async Task<List<string>> GetImages()
        {
            var images = await Js.InvokeAsync<List<List<ImageModel>>>("getImages");
            return images
                .SelectMany(toFlatten => toFlatten.ToList())
                .Select(image => Convert.ToBase64String(image.Data))
                .ToList();
        }
    }
}
