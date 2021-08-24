using Couple.Client.States.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Issue
{
    public abstract class CreateUpdateIssueBase : ComponentBase, IDisposable
    {
        [Inject] protected HttpClient HttpClient { get; init; }

        [Inject] protected NavigationManager NavigationManager { get; init; }

        [Inject] protected IssueStateContainer IssueStateContainer { get; init; }
        [Inject] protected CreateUpdateIssueStateContainer CreateUpdateIssueStateContainer { get; init; }

        [Inject] protected IJSRuntime Js { get; init; }

        protected abstract Task Save();
        public abstract void Dispose();
    }
}
