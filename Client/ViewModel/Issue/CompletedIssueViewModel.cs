using Couple.Shared.Model;
using System;
using System.Collections.Generic;

namespace Couple.Client.ViewModel.Issue
{
    public class CompletedIssueViewModel
    {
        public string Title { get; }
        public For For { get; }
        public List<string> Tasks { get; }
        public DateTime CompletedOn { get; }

        public CompletedIssueViewModel(string title, For @for, List<string> tasks, DateTime completedOn)
            => (Title, For, Tasks, CompletedOn) = (title, @for, tasks, completedOn);
    }
}
