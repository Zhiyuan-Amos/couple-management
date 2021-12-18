using Couple.Client.States.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Issue.Components;

public partial class IssueCreateUpdateForm
{
    [EditorRequired] [Parameter] public Func<Task> OnSaveCallback { get; set; }

    [CascadingParameter(Name = "CreateUpdateIssueStateContainer")]
    private CreateUpdateIssueStateContainer CreateUpdateIssueStateContainer { get; init; }

    private IReadOnlyList<IReadOnlyTaskViewModel> Tasks { get; set; }

    private bool IsAddNewTaskEnabled => Tasks.All(task => task.Content.Any());

    private bool IsSaveEnabled => CreateUpdateIssueStateContainer.Title.Any()
                                  && Tasks.Any(task => task.Content.Any());

    protected override void OnInitialized() => Tasks = CreateUpdateIssueStateContainer.Tasks;

    private void OnForChange(For @for) => CreateUpdateIssueStateContainer.For = @for;

    private void AddNewTask() => CreateUpdateIssueStateContainer.AddTask("");

    private void SetContent(int index, string content) => CreateUpdateIssueStateContainer.SetContent(index, content);

    private void Save()
    {
        CreateUpdateIssueStateContainer.RemoveEmptyTasks();
        OnSaveCallback();
    }
}
