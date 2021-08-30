using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Shared.Model;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Issue
{
    public class CreateIssueBase : CreateUpdateIssueBase
    {
        protected override void OnInitialized()
        {
            CreateUpdateIssueStateContainer.Initialize("",
                For.Him,
                new List<TaskModel>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Content = "",
                        IsCompleted = false,
                    },
                });
        }

        protected override async Task Save()
        {
            var id = Guid.NewGuid();
            var toPersist = new IssueModel
            {
                Id = id,
                Title = CreateUpdateIssueStateContainer.Title,
                For = CreateUpdateIssueStateContainer.For,
                Tasks = IssueAdapter.ToTaskModel(CreateUpdateIssueStateContainer.Tasks),
                CreatedOn = DateTime.Now,
            };
            await Js.InvokeVoidAsync("addIssue", toPersist);

            IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");
            NavigationManager.NavigateTo("/todo");

            var toCreate = IssueAdapter.ToCreateDto(toPersist);
            await HttpClient.PostAsJsonAsync($"api/Issues", toCreate);
        }

        public override void Dispose()
        {
            CreateUpdateIssueStateContainer.Reset();
        }
    }
}
