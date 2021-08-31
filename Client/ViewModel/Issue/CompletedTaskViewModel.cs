using Couple.Shared.Model;
using System;

namespace Couple.Client.ViewModel.Issue
{
    public class CompletedTaskViewModel
    {
        public Guid Id { get; }
        public For For { get; }
        public string Content { get; }

        public Guid IssueId { get; }
        public string IssueTitle { get; }
        public DateTime CreatedOn { get; }

        public CompletedTaskViewModel(Guid id, For @for, string content, Guid issueId, string issueTitle,
            DateTime createdOn) => (Id, For, Content, IssueId, IssueTitle, CreatedOn) =
            (id, @for, content, issueId, issueTitle, createdOn);
    }
}
