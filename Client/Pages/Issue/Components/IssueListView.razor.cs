using System.Net.Http.Json;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Couple.Client.States.Issue;
using Couple.Client.Utility;

namespace Couple.Client.Pages.Issue.Components;

public partial class IssueListView
{
    [Inject] private DbContextProvider DbContextProvider { get; } = default!;
    [Inject] private IssueStateContainer IssueStateContainer { get; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; } = default!;
    [Inject] private HttpClient HttpClient { get; } = default!;
    [Inject] private NavigationManager NavigationManager { get; } = default!;

    [EditorRequired] [Parameter] public List<IReadOnlyIssueModel> Issues { get; set; } = default!;

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
        var doneIssueModel = await CompleteTaskHelper.CompleteTaskAsync(issue, task.Id, date, db);

        IssueStateContainer.UpdateIssue(issue);
        DoneStateContainer.UpdateIssue(doneIssueModel);

        var toUpdate =
            IssueAdapter.ToCompleteDto(new(task.Id, issue.Id, date));
        await HttpClient.PutAsJsonAsync("api/Tasks/Complete", toUpdate);
    }
}
