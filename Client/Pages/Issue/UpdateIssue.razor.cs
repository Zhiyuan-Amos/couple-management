using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Pages.Issue;

public class UpdateIssueBase : CreateUpdateIssueBase
{
    private IReadOnlyIssueModel? _currentIssueModel;
    [EditorRequired] [Parameter] public Guid IssueId { get; set; }

    protected override void OnInitialized()
    {
        if (!IssueStateContainer.TryGetIssue(IssueId, out _currentIssueModel))
        {
            NavigationManager.NavigateTo("/todo");
            return;
        }

        CreateUpdateIssueStateContainer = new(_currentIssueModel.Title,
            _currentIssueModel.For,
            _currentIssueModel.ReadOnlyTasks);
    }

    protected async Task Delete()
    {
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        db.Issues.Remove(new(IssueId));
        await db.SaveChangesAsync();
        IssueStateContainer.Issues = await db.Issues.ToListAsync();

        NavigationManager.NavigateTo("/todo");

        await HttpClient.DeleteAsync($"api/Issues/{IssueId}");
    }

    protected override async Task Save()
    {
        var issue = new IssueModel(_currentIssueModel.Id, _currentIssueModel.Title, _currentIssueModel.For,
            _currentIssueModel.ReadOnlyTasks, _currentIssueModel.CreatedOn);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();

        db.Attach(issue);
        issue.Title = CreateUpdateIssueStateContainer.Title;
        issue.For = CreateUpdateIssueStateContainer.For;
        issue.Tasks = IssueAdapter.ToTaskModel(CreateUpdateIssueStateContainer.Tasks);
        await db.SaveChangesAsync();

        IssueStateContainer.Issues = await db.Issues.ToListAsync();
        NavigationManager.NavigateTo("/todo");

        var toUpdate = IssueAdapter.ToUpdateDto(issue);
        await HttpClient.PutAsJsonAsync("api/Issues", toUpdate);
    }
}
