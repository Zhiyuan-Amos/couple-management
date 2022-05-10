using Couple.Client.Model.Issue;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Pages.Issue;

public partial class Issue : IDisposable
{
    private static bool s_isDataLoaded;
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;

    [Inject] private IssueStateContainer IssueStateContainer { get; init; } = default!;

    private List<IReadOnlyIssueModel> Issues =>
        IssueStateContainer.Issues
            .OrderByDescending(issue => issue.CreatedOn)
            .ToList();

    public void Dispose() => IssueStateContainer.OnChange -= StateHasChanged;

    protected override async Task OnInitializedAsync()
    {
        IssueStateContainer.OnChange += StateHasChanged;

        if (s_isDataLoaded)
        {
            return;
        }

        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        IssueStateContainer.Issues = await db.Issues.ToListAsync();

        s_isDataLoaded = true;
    }

    private void AddIssue() => NavigationManager.NavigateTo("/todo/create");
}
