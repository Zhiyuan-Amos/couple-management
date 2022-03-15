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

    public void AddIssue(IssueModel issue)
    {
        _issues.Add(issue);
        _idToIssue.Add(issue.Id, issue);

        NotifyStateChanged();
    }

    public void UpdateIssue(IssueModel issue)
    {
        _idToIssue.Remove(issue.Id, out var oldIssue);
        _issues.Remove(oldIssue!);

        _issues.Add(issue);
        _idToIssue.Add(issue.Id, issue);

        NotifyStateChanged();
    }

    public void DeleteIssue(Guid issueId)
    {
        _idToIssue.Remove(issueId, out var oldIssue);
        _issues.Remove(oldIssue!);

        NotifyStateChanged();
    }

    public bool TryGetIssue(Guid id, out IReadOnlyIssueModel? readOnlyIssue) =>
        _idToIssue.TryGetValue(id, out readOnlyIssue);
}
