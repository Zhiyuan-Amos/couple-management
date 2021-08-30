using Couple.Shared.Model;
using System;
using System.Collections.Generic;

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
