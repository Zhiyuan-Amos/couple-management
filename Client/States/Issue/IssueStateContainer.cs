using Couple.Client.Model.Issue;

namespace Couple.Client.States.Issue;

public class IssueStateContainer : Notifier
{
    private readonly List<IReadOnlyIssueModel> _issues = new();
    private Dictionary<Guid, IReadOnlyIssueModel> _idToIssue = new();

    public IReadOnlyList<IReadOnlyIssueModel> Issues
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

    public bool TryGetIssue(Guid id, out IReadOnlyIssueModel readOnlyIssue)
    {
        if (!_idToIssue.TryGetValue(id, out readOnlyIssue))
        {
            return false;
        }

        return true;
    }
}
