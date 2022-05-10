using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model;

namespace Couple.Client.States.Issue;

public class CreateUpdateIssueStateContainer
{
    private readonly List<CreateUpdateTaskViewModel> _tasks;

    public CreateUpdateIssueStateContainer(string title, For @for, IEnumerable<IReadOnlyTaskModel> tasks)
    {
        Title = title;
        For = @for;
        _tasks = IssueAdapter.ToCreateUpdateTaskViewModel(tasks);
    }

    public string Title { get; set; }
    public For For { get; set; }

    public IReadOnlyList<IReadOnlyTaskViewModel> Tasks => _tasks;

    public void AddTask(string content) => _tasks.Add(new(content));

    public void RemoveEmptyTasks() => _tasks.RemoveAll(task => !task.Content.Any());

    public void SetContent(int index, string content) => _tasks[index].Content = content;
}