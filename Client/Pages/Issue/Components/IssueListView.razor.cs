using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Issue;
using Couple.Client.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Pages.Issue.Components;

public partial class IssueListView
{
    [Inject] protected DbContextProvider DbContextProvider { get; init; }
    [Inject] protected IssueStateContainer? IssueStateContainer { get; init; }
    [Inject] protected HttpClient? HttpClient { get; init; }
    [Inject] private NavigationManager? NavigationManager { get; init; }

    [EditorRequired] [Parameter] public List<IReadOnlyIssueModel>? Issues { get; set; }

    private void EditIssue(IReadOnlyIssueModel selectedIssue) =>
        NavigationManager.NavigateTo($"/todo/{selectedIssue.Id}");

    private async Task OnCheckboxToggle(Guid id, IReadOnlyTaskModel task)
    {
        var readOnlyIssue = Issues.Single(x => x.Id == id);
        var issue = new IssueModel(readOnlyIssue.Id, readOnlyIssue.Title, readOnlyIssue.For,
            readOnlyIssue.ReadOnlyTasks,
            readOnlyIssue.CreatedOn);
        var date = DateOnly.FromDateTime(DateTime.Now);
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        await CompleteTaskHelper.CompleteTaskAsync(issue, task.Id, date, db);

        IssueStateContainer.Issues = await db.Issues.ToListAsync();

        var toUpdate =
            IssueAdapter.ToCompleteDto(new(task.Id, issue.Id, date));
        await HttpClient.PutAsJsonAsync("api/Tasks/Complete", toUpdate);
    }
}
