using Couple.Client.Model.Issue;
using Couple.Client.States.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Couple.Client.Pages.Issue;

public partial class Issue : IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; init; }

    [Inject] private IJSRuntime Js { get; init; }

    [Inject] private IssueStateContainer IssueStateContainer { get; init; }

    private List<IssueModel> Issues
    {
        get
        {
            return IssueStateContainer.Issues
                .OrderByDescending(issue => issue.CreatedOn)
                .ToList();
        }
    }

    public void Dispose()
    {
        IssueStateContainer.OnChange -= StateHasChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        IssueStateContainer.OnChange += StateHasChanged;
        IssueStateContainer.Issues = await Js.InvokeAsync<List<IssueModel>>("getIssues");
    }

    private void AddIssue()
    {
        NavigationManager.NavigateTo("/todo/create");
    }
}
