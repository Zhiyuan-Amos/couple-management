using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Issue
{
    public class UpdateIssueBase : CreateUpdateIssueBase
    {
        [Parameter] public Guid IssueId { get; set; }
        private IssueModel _currentIssueModel;

        protected override void OnInitialized()
        {
            if (!IssueStateContainer.TryGetIssue(IssueId, out _currentIssueModel))
            {
                NavigationManager.NavigateTo("/todo");
                return;
            }

            CreateUpdateIssueStateContainer = new(_currentIssueModel.Title,
                _currentIssueModel.For,
                _currentIssueModel.Tasks);
        }

        protected async Task Delete()
        {
            await Js.InvokeVoidAsync("deleteIssue", IssueId);
            IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");

            NavigationManager.NavigateTo("/todo");

            await HttpClient.DeleteAsync($"api/Issues/{IssueId}");
        }

        protected override async Task Save()
        {
            var toPersist = new IssueModel
            {
                Id = IssueId,
                Title = CreateUpdateIssueStateContainer.Title,
                For = CreateUpdateIssueStateContainer.For,
                Tasks = IssueAdapter.ToTaskModel(CreateUpdateIssueStateContainer.Tasks),
                CreatedOn = _currentIssueModel.CreatedOn,
            };
            await Js.InvokeVoidAsync("updateIssue", toPersist);

            IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");
            NavigationManager.NavigateTo("/todo");

            var toUpdate = IssueAdapter.ToUpdateDto(toPersist);
            await HttpClient.PutAsJsonAsync("api/Issues", toUpdate);
        }
    }
}
