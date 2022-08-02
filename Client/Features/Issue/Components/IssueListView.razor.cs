using System.Net.Http.Json;
using Couple.Client.Features.Done.States;
using Couple.Client.Features.Issue.Adapters;
using Couple.Client.Features.Issue.Models;
using Couple.Client.Features.Issue.States;
using Couple.Client.Features.Synchronizer;
using Couple.Client.Shared.Helpers;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Issue.Components;

public partial class IssueListView : IDisposable
{
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private IssueStateContainer IssueStateContainer { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;
    [Inject] private HttpClient HttpClient { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    
    public void Dispose() => IssueStateContainer.OnChange -= StateHasChanged;

    private IReadOnlyList<IReadOnlyIssueModel> _issues = new List<IReadOnlyIssueModel>();

    private List<IReadOnlyIssueModel> Issues
    {
        get
        {
            _issues = IssueStateContainer.Issues;
            return _issues
                .OrderByDescending(issue => issue.CreatedOn)
                .ToList();
        }
    } 

    protected override void OnInitialized() => 
        IssueStateContainer.OnChange += StateHasChanged;

    private void EditIssue(IReadOnlyIssueModel selectedIssue) =>
        NavigationManager.NavigateTo($"/todo/{selectedIssue.Id}");

    private async Task OnCheckboxToggle(Guid id, IReadOnlyTaskModel task)
    {
        var readOnlyIssue = _issues.First(x => x.Id == id);
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
        await HttpClient.PutAsJsonAsync("api/Task", toUpdate);
    }
}
