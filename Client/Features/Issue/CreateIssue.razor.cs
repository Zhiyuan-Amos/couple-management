using System.Net.Http.Json;
using Couple.Client.Features.Issue.Adapters;
using Couple.Client.Features.Issue.Models;
using Couple.Shared.Models;

namespace Couple.Client.Features.Issue;

public class CreateIssueBase : CreateUpdateIssueBase
{
    protected override void OnInitialized() =>
        CreateUpdateIssueStateContainer = new("",
            For.Him,
            new List<TaskModel> { new("") });

    protected override async Task Save()
    {
        var toPersist = new IssueModel(CreateUpdateIssueStateContainer.Title,
            CreateUpdateIssueStateContainer.For,
            IssueAdapter.ToTaskModel(CreateUpdateIssueStateContainer.Tasks),
            DateTime.Now);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();

        db.Issues.Add(toPersist);
        await db.SaveChangesAsync();

        IssueStateContainer.AddIssue(toPersist);
        NavigationManager.NavigateTo("/todo");

        var toCreate = IssueAdapter.ToCreateDto(toPersist);
        await HttpClient.PostAsJsonAsync("api/Issue", toCreate);
    }
}
