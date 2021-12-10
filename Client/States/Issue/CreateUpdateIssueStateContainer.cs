using System;
using System.Collections.Generic;
using System.Linq;
using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model;

namespace Couple.Client.States.Issue;

public class CreateUpdateIssueStateContainer
{
    public string Title { get; set; }
    public For For { get; set; }
    private readonly List<CreateUpdateTaskViewModel> _tasks;

    public IReadOnlyList<IReadOnlyTaskViewModel> Tasks => _tasks;

    public void AddTask(string content)
    {
        _tasks.Add(new(Guid.NewGuid(), content));
    }

    public void RemoveEmptyTasks() => _tasks.RemoveAll(task => !task.Content.Any());

    public void SetContent(int index, string content)
    {
        _tasks[index].Content = content;
    }

    public CreateUpdateIssueStateContainer(string title, For @for, IEnumerable<TaskModel> tasks)
    {
        Title = title;
        For = @for;
        _tasks = IssueAdapter.ToCreateUpdateTaskViewModel(tasks);
    }
}
