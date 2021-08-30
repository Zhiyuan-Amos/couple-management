using Couple.Client.Adapters;
using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.Issue
{
    public class CreateUpdateIssueStateContainer
    {
        public string Title { get; set; }
        public For For { get; set; }
        private List<CreateUpdateTaskViewModel> _tasks;

        public IReadOnlyList<IReadOnlyTaskViewModel> Tasks => _tasks;

        public void AddTask(string content, bool isCompleted)
        {
            _tasks.Add(new()
            {
                Id = Guid.NewGuid(),
                Content = content,
                IsCompleted = isCompleted,
            });
        }

        public void RemoveEmptyTasks() => _tasks.RemoveAll(task => !task.Content.Any());

        public void SetContent(int index, string content)
        {
            _tasks[index].Content = content;
        }

        public void Initialize(string name, For @for, IEnumerable<TaskModel> tasks)
        {
            Title = name;
            For = @for;
            _tasks = IssueAdapter.ToCreateUpdateTaskViewModel(tasks);
        }

        public void Reset()
        {
            Title = null;
            For = For.Him;
            _tasks = new();
        }
    }
}
