using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Shared.Model;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Issue;

public class CreateIssueBase : CreateUpdateIssueBase
{
    protected override void OnInitialized()
    {
        CreateUpdateIssueStateContainer = new("",
            For.Him,
            new List<TaskModel>
            {
                new(Guid.NewGuid(), ""),
            });
    }

    protected override async Task Save()
    {
        var id = Guid.NewGuid();
        var toPersist = new IssueModel(id,
            CreateUpdateIssueStateContainer.Title,
            CreateUpdateIssueStateContainer.For,
            IssueAdapter.ToTaskModel(CreateUpdateIssueStateContainer.Tasks),
            DateTime.Now);
        await Js.InvokeVoidAsync("createIssue", toPersist);

        IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");
        NavigationManager.NavigateTo("/todo");

        var toCreate = IssueAdapter.ToCreateDto(toPersist);
        await HttpClient.PostAsJsonAsync($"api/Issues", toCreate);
    }
}
