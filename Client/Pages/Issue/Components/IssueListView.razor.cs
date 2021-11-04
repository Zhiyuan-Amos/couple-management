using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Client.States.Issue;
using Couple.Client.ViewModel.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Issue.Components
{
    public partial class IssueListView
    {
        [Inject] protected IssueStateContainer IssueStateContainer { get; init; }
        [Inject] protected HttpClient HttpClient { get; init; }
        [Inject] private NavigationManager NavigationManager { get; init; }
        [Inject] private IJSRuntime Js { get; init; }

        [Parameter] public List<IssueModel> Issues { get; set; }

        private void EditIssue(IssueModel selectedIssue) => NavigationManager.NavigateTo($"/todo/{selectedIssue.Id}");

        private async Task OnCheckboxToggle(Guid id, TaskModel task)
        {
            var viewModel = Issues.Single(x => x.Id == id);

            var toPersist = new CreateCompletedTaskModel
            {
                Id = task.Id,
                For = viewModel.For,
                Content = task.Content,
                IssueId = id,
                IssueTitle = viewModel.Title,
                CreatedOn = DateTime.Now,
            };

            await Js.InvokeVoidAsync("completeTask", toPersist);
            IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");

            var toUpdate = IssueAdapter.ToCompleteDto(toPersist);
            await HttpClient.PutAsJsonAsync("api/Tasks/Complete", toUpdate);
        }
    }
}
