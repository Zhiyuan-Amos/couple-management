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

        private List<CompletedIssueViewModel> Issues
        {
            get
            {
                var orderedIssues = IssueStateContainer.CompletedIssues
                    .OrderBy(issue => issue.CompletedOn)
                    .ToList();
                return IssueAdapter.ToCompletedViewModel(orderedIssues);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IssueStateContainer.CompletedIssues = await Js.InvokeAsync<List<CompletedIssueModel>>("getCompletedIssues");
        }
    }
}
