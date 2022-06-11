using Couple.Client.Features.Issue.States;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Features.Issue;

public partial class Issue
{
    private static bool s_isDataLoaded;
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;

    [Inject] private IssueStateContainer IssueStateContainer { get; init; } = default!;

    protected override async Task OnInitializedAsync()
    {
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
