using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couple.Client.Model.Issue;
using Couple.Client.States.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Issue
{
    public partial class Issue : IDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; init; }

        [Inject] private IJSRuntime Js { get; init; }

        [Inject] private IssueStateContainer IssueStateContainer { get; init; }

        private List<IssueModel> Issues
        {
            get
            {
                return IssueStateContainer.Issues
                    .OrderByDescending(issue => issue.CreatedOn)
                    .ToList();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IssueStateContainer.OnChange += StateHasChanged;
            IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");
        }

        public void Dispose() => IssueStateContainer.OnChange -= StateHasChanged;

        private void AddIssue() => NavigationManager.NavigateTo($"/todo/create");
    }
}
