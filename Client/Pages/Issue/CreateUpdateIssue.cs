using Couple.Client.States.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Issue;

public abstract class CreateUpdateIssueBase : ComponentBase
{
    [Inject] protected HttpClient HttpClient { get; init; }

    [Inject] protected NavigationManager NavigationManager { get; init; }

    [Inject] protected IssueStateContainer IssueStateContainer { get; init; }
    [Inject] protected IJSRuntime Js { get; init; }

    protected CreateUpdateIssueStateContainer CreateUpdateIssueStateContainer { get; set; }
    protected abstract Task Save();
}
