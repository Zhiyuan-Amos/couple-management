﻿using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Issue;
using Couple.Shared.Model.Event;
using Couple.Shared.Model.Issue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.Adapters
{
    public static class IssueAdapter
    {
        public static List<TaskViewModel> ToTaskViewModel(IEnumerable<TaskModel> models) =>
            models.Select(ToTaskViewModel).ToList();

        public static TaskViewModel ToTaskViewModel(TaskModel model) => new()
        {
            Id = model.Id,
            Content = model.Content,
        };

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

        public static TaskModel ToTaskModel(TaskViewModel model) => new()
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

        public static List<TaskDto> ToTaskDto(IEnumerable<TaskViewModel> models) =>
            models.Select(ToTaskDto).ToList();

        public static TaskDto ToTaskDto(TaskViewModel model) => new()
        {
            Content = model.Content,
        };

        public static List<TaskDto> ToTaskDto(IEnumerable<TaskModel> models) =>
            models.Select(ToTaskDto).ToList();

        public static TaskDto ToTaskDto(TaskModel model) => new()
        {
            Id = model.Id,
            Content = model.Content,
        };

        public static List<IssueViewModel> ToViewModel(IEnumerable<IssueModel> models) =>
            models.Select(ToViewModel).ToList();

        public static IssueViewModel ToViewModel(IssueModel model) =>
            new(model.Id, model.Title, model.For, model.Tasks.Select(ToTaskViewModel).ToList(), model.CreatedOn);

        public static List<IssueModel> ToModel(IEnumerable<IssueDto> models) => models.Select(ToModel).ToList();

        public static IssueModel ToModel(IssueDto model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = ToTaskModel(model.Tasks),
            CreatedOn = model.CreatedOn,
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

        public static List<IssueModel> ToModel(IEnumerable<IssueViewModel> models) => models.Select(ToModel).ToList();

        public static IssueModel ToModel(IssueViewModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = model.Tasks.Select(ToTaskModel).ToList(),
            CreatedOn = model.CreatedOn,
        };

        public static CompletedTaskModel ToCompletedModel(CompleteTaskDto model) => new()
        {
            Id = model.Id,
            For = model.For,
            Content = model.Content,
            IssueId = model.IssueId,
            IssueTitle = model.IssueTitle,
            CreatedOn = model.CreatedOn,
        };

        public static List<CompletedTaskViewModel> ToCompletedViewModel(IEnumerable<CompletedTaskModel> models) =>
            models.Select(ToCompletedViewModel).ToList();

        public static CompletedTaskViewModel ToCompletedViewModel(CompletedTaskModel model) => new(model.Id,
            model.For, model.Content, model.IssueId, model.IssueTitle, model.CreatedOn);

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

        public static CompleteTaskDto ToCompleteDto(CompletedTaskModel model) => new()
        {
            Id = model.Id,
            For = model.For,
            Content = model.Content,
            IssueId = model.IssueId,
            IssueTitle = model.IssueTitle,
            CreatedOn = model.CreatedOn,
        };

        public static List<IssueDto> ToDto(IEnumerable<IssueViewModel> models) => models.Select(ToDto).ToList();

        public static IssueDto ToDto(IssueViewModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = ToTaskDto(model.Tasks),
            CreatedOn = model.CreatedOn,
        };

        public static List<IssueDto> ToDto(IEnumerable<IssueModel> models) => models.Select(ToDto).ToList();

        public static IssueDto ToDto(IssueModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = ToTaskDto(model.Tasks),
            CreatedOn = model.CreatedOn,
        };
    }
}