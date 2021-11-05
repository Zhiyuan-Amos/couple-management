using System;
using System.Collections.Generic;
using System.Linq;
using Couple.Client.Model.Issue;

namespace Couple.Client.States.Issue
{
    public class IssueStateContainer : Notifier
    {
        private readonly List<IssueModel> _issues = new();
        private Dictionary<Guid, IssueModel> _idToIssue = new();

        public IReadOnlyList<IssueModel> Issues
        {
            get => _issues.AsReadOnly();
            set
            {
                _issues.Clear();
                _issues.AddRange(value);

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
    }
}
