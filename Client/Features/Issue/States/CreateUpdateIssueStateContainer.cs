using Couple.Client.Features.Issue.Adapters;
using Couple.Client.Features.Issue.Models;
using Couple.Client.Features.Issue.ViewModels;
using Couple.Shared.Models;

namespace Couple.Client.Features.Issue.States;

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
