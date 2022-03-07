using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model.Issue;

namespace Couple.Client.Adapters;

public static class IssueAdapter
{
    public static List<TaskModel> ToTaskModel(IEnumerable<TaskDto> models) => models.Select(ToTaskModel).ToList();

    public static TaskModel ToTaskModel(TaskDto model) => new(model.Id, model.Content);

    public static List<TaskModel> ToTaskModel(IEnumerable<IReadOnlyTaskViewModel> models) =>
        models.Select(ToTaskModel).ToList();

    public static TaskModel ToTaskModel(IReadOnlyTaskViewModel model) => new(model.Id, model.Content);

    public static List<CreateUpdateTaskViewModel>
        ToCreateUpdateTaskViewModel(IEnumerable<IReadOnlyTaskModel> models) =>
        models.Select(ToCreateUpdateTaskViewModel)
            .ToList();

    public static CreateUpdateTaskViewModel ToCreateUpdateTaskViewModel(IReadOnlyTaskModel model) =>
        new(model.Id, model.Content);

    public static TaskDto ToTaskDto(TaskModel model) => new(model.Id, model.Content);

    public static IssueModel ToModel(CreateIssueDto model) =>
        new(model.Id, model.Title, model.For,
            ToTaskModel(model.Tasks), model.CreatedOn);

    public static IssueModel ToModel(UpdateIssueDto model) =>
        new(model.Id, model.Title, model.For,
            ToTaskModel(model.Tasks), model.CreatedOn);

    public static CreateCompletedTaskModel ToCompletedModel(CompleteTaskDto model) =>
        new(model.TaskId, model.IssueId, model.CompletedDate);

    public static CreateIssueDto ToCreateDto(IssueModel model) =>
        new(model.Id, model.Title, model.For,
            model.Tasks.Select(ToTaskDto).ToList(), model.CreatedOn);

    public static UpdateIssueDto ToUpdateDto(IssueModel model) =>
        new(model.Id, model.Title, model.For,
            model.Tasks.Select(ToTaskDto).ToList(), model.CreatedOn);

    public static CompleteTaskDto ToCompleteDto(CreateCompletedTaskModel model) =>
        new(model.TaskId, model.IssueId, model.CompletedDate);
}
