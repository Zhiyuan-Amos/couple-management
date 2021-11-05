using System.Collections.Generic;
using System.Linq;
using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model.Issue;

namespace Couple.Client.Adapters
{
    public static class IssueAdapter
    {
        public static List<TaskModel> ToTaskModel(IEnumerable<TaskDto> models) =>
            models.Select(ToTaskModel).ToList();

        public static TaskModel ToTaskModel(TaskDto model) => new()
        {
            Id = model.Id,
            Content = model.Content,
        };

        public static List<TaskModel> ToTaskModel(IEnumerable<IReadOnlyTaskViewModel> models) =>
            models.Select(ToTaskModel).ToList();

        public static TaskModel ToTaskModel(IReadOnlyTaskViewModel model) => new()
        {
            Id = model.Id,
            Content = model.Content,
        };

        public static List<CreateUpdateTaskViewModel>
            ToCreateUpdateTaskViewModel(IEnumerable<TaskModel> models) =>
            models.Select(ToCreateUpdateTaskViewModel)
                .ToList();

        public static CreateUpdateTaskViewModel ToCreateUpdateTaskViewModel(TaskModel model) => new()
        {
            Id = model.Id,
            Content = model.Content,
        };

        public static TaskDto ToTaskDto(TaskModel model) => new()
        {
            Id = model.Id,
            Content = model.Content,
        };

        public static IssueModel ToModel(CreateIssueDto model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = ToTaskModel(model.Tasks),
            CreatedOn = model.CreatedOn,
        };

        public static IssueModel ToModel(UpdateIssueDto model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = ToTaskModel(model.Tasks),
            CreatedOn = model.CreatedOn,
        };

        public static CreateCompletedTaskModel ToCompletedModel(CompleteTaskDto model) => new()
        {
            Id = model.Id,
            For = model.For,
            Content = model.Content,
            IssueId = model.IssueId,
            IssueTitle = model.IssueTitle,
            CreatedOn = model.CreatedOn,
        };

        public static CreateIssueDto ToCreateDto(IssueModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = model.Tasks.Select(ToTaskDto).ToList(),
            CreatedOn = model.CreatedOn,
        };

        public static UpdateIssueDto ToUpdateDto(IssueModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = model.Tasks.Select(ToTaskDto).ToList(),
            CreatedOn = model.CreatedOn,
        };

        public static CompleteTaskDto ToCompleteDto(CreateCompletedTaskModel model) => new()
        {
            Id = model.Id,
            For = model.For,
            Content = model.Content,
            IssueId = model.IssueId,
            IssueTitle = model.IssueTitle,
            CreatedOn = model.CreatedOn,
        };
    }
}
