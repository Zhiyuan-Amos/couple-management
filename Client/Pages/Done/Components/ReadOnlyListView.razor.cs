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

        private List<CompletedTaskViewModel> Tasks
        {
            get
            {
                var orderedTasks = IssueStateContainer.CompletedTasks
                    .OrderByDescending(issue => issue.CreatedOn)
                    .ToList();
                return IssueAdapter.ToCompletedViewModel(orderedTasks);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IssueStateContainer.CompletedTasks = await Js.InvokeAsync<List<CompletedTaskModel>>("getCompletedTasks");
        }
    }
}
