using System;
using System.Collections.Generic;
using Couple.Shared.Model;

namespace Couple.Client.ViewModel.Issue
{
    public class IssueViewModel
    {
        public Guid Id { get; }
        public string Title { get; }
        public For For { get; }
        public List<TaskViewModel> Tasks { get; }
        public DateTime CreatedOn { get; }

        public IssueViewModel(Guid id, string title, For @for, List<TaskViewModel> tasks, DateTime createdOn)
            => (Id, Title, For, Tasks, CreatedOn) = (id, title, @for, tasks, createdOn);
    }
}
