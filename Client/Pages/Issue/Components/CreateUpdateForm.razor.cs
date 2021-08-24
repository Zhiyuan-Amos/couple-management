using Couple.Client.States.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Couple.Client.Pages.Issue.Components
{
    public partial class CreateUpdateForm
    {
        [Parameter] public Func<Task> OnSaveCallback { get; set; }

        [Inject] protected CreateUpdateIssueStateContainer CreateUpdateIssueStateContainer { get; init; }

        private IReadOnlyList<IReadOnlyTaskViewModel> Tasks { get; set; }

        protected override void OnInitialized()
        {
            Tasks = CreateUpdateIssueStateContainer.Tasks;
        }

        private void OnForChange(For @for) => CreateUpdateIssueStateContainer.For = @for;

        private bool IsAddNewTaskEnabled => Tasks.All(task => task.Content.Any());

        private void AddNewTask() => CreateUpdateIssueStateContainer.AddTask("", false);
        private void SetContent(int index, string content) => CreateUpdateIssueStateContainer.SetContent(index, content);

        private void Save()
        {
            CreateUpdateIssueStateContainer.TrimTasks();
            OnSaveCallback();
        }

        private bool IsSaveEnabled => Tasks.Any(task => task.Content.Any());
    }
}
