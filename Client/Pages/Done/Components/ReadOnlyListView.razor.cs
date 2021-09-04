using Couple.Client.Adapters;
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
                var dictionary = IssueAdapter.ToCompletedViewModel(IssueStateContainer.CompletedTasks)
                    .GroupBy(task => DateOnly.FromDateTime(task.CreatedOn.Date))
                    .ToDictionary(grouping => grouping.Key,
                        grouping => grouping.OrderByDescending(issue => issue.CreatedOn)
                            .ToList());
                return new(dictionary);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IssueStateContainer.CompletedTasks = await Js.InvokeAsync<List<CompletedTaskModel>>("getCompletedTasks");
        }
    }
}
