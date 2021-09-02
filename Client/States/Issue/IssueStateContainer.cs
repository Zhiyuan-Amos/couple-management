using Couple.Client.Model.Issue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.Issue
{
    public class IssueStateContainer : Notifier
    {
        private List<IssueModel> _issues = new();
        private Dictionary<Guid, IssueModel> _idToIssue = new();
        private List<CompletedTaskModel> _completedTasks = new();

        public IReadOnlyList<IssueModel> Issues
        {
            get => _issues.AsReadOnly();
            set
            {
                _issues = value.ToList();
                _idToIssue = value.ToDictionary(issue => issue.Id);
                NotifyStateChanged();
            }
        }

        public bool TryGetIssue(Guid id, out IssueModel issue)
        {
            if (!_idToIssue.TryGetValue(id, out issue))
            {
                return false;
            }

            return true;
        }

        public IReadOnlyList<CompletedTaskModel> CompletedTasks
        {
            get => _completedTasks.AsReadOnly();
            set
            {
                _completedTasks = value.ToList();
                NotifyStateChanged();
            }
        }
    }
}
