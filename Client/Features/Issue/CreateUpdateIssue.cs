using Couple.Client.Features.Issue.States;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Issue;

public abstract class CreateUpdateIssueBase : ComponentBase
{
    [Inject] protected HttpClient HttpClient { get; init; } = default!;

    [Inject] protected NavigationManager NavigationManager { get; init; } = default!;

    [Inject] protected IssueStateContainer IssueStateContainer { get; init; } = default!;

    [Inject] protected DbContextProvider DbContextProvider { get; init; } = default!;

    protected CreateUpdateIssueStateContainer CreateUpdateIssueStateContainer { get; set; } = default!;
    protected abstract Task Save();
}
