using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Pages.Issue;

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

        IssueStateContainer.Issues = await db.Issues.ToListAsync();
        NavigationManager.NavigateTo("/todo");

        var toCreate = IssueAdapter.ToCreateDto(toPersist);
        await HttpClient.PostAsJsonAsync("api/Issues", toCreate);
    }
}
