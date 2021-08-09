using Couple.Client.Model.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model.Event;
using Couple.Shared.Model.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.Adapters
{
    public static class ToDoAdapter
    {
        public static ToDoInnerViewModel ToInnerViewModel(ToDoInnerModel model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static List<ToDoInnerModel> ToInnerModel(IEnumerable<ToDoInnerDto> models) =>
            models.Select(ToInnerModel).ToList();

        public static ToDoInnerModel ToInnerModel(ToDoInnerDto model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static List<ToDoInnerModel> ToInnerModel(IEnumerable<IReadOnlyInnerViewModel> models) =>
            models.Select(ToInnerModel).ToList();

        public static ToDoInnerModel ToInnerModel(IReadOnlyInnerViewModel model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static ToDoInnerModel ToInnerModel(ToDoInnerViewModel model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static List<CreateUpdateInnerViewModel>
            ToCreateUpdateInnerViewModel(IEnumerable<ToDoInnerModel> models) =>
            models.Select(ToCreateUpdateInnerViewModel)
                .ToList();

        public static CreateUpdateInnerViewModel ToCreateUpdateInnerViewModel(ToDoInnerModel model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static List<ToDoInnerDto> ToInnerDto(IEnumerable<ToDoInnerViewModel> models) =>
            models.Select(ToInnerDto).ToList();

        public static ToDoInnerDto ToInnerDto(ToDoInnerViewModel model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static List<ToDoInnerDto> ToInnerDto(IEnumerable<ToDoInnerModel> models) =>
            models.Select(ToInnerDto).ToList();

        public static ToDoInnerDto ToInnerDto(ToDoInnerModel model) => new()
        {
            Content = model.Content,
            IsCompleted = model.IsCompleted,
        };

        public static List<ToDoViewModel> ToViewModel(IEnumerable<ToDoModel> models) =>
            models.Select(ToViewModel).ToList();

        public static ToDoViewModel ToViewModel(ToDoModel model) =>
            new(model.Id, model.Name, model.For, model.ToDos.Select(ToInnerViewModel).ToList(), model.CreatedOn);

        public static List<ToDoModel> ToModel(IEnumerable<ToDoDto> models) => models.Select(ToModel).ToList();

        public static ToDoModel ToModel(ToDoDto model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = ToInnerModel(model.ToDos),
            CreatedOn = model.CreatedOn,
        };

        public static List<ToDoModel> ToModel(IEnumerable<ToDoViewModel> models) => models.Select(ToModel).ToList();

        public static ToDoModel ToModel(ToDoViewModel model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = model.ToDos.Select(ToInnerModel).ToList(),
            CreatedOn = model.CreatedOn,
        };

        public static CompletedToDoModel ToCompletedModel(ToDoViewModel model, DateTime completedOn) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = model.ToDos.Select(ToInnerModel).ToList(),
            CreatedOn = model.CreatedOn,
            CompletedOn = completedOn,
        };

        public static List<CompletedToDoViewModel> ToCompletedViewModel(IEnumerable<CompletedToDoModel> models) =>
            models.Select(ToCompletedViewModel).ToList();

        public static CompletedToDoViewModel ToCompletedViewModel(CompletedToDoModel model) => new(model.Name,
            model.For, model.ToDos.Select(innerModel => innerModel.Content).ToList(), model.CompletedOn);

        public static CreateToDoDto ToCreateDto(ToDoModel model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = model.ToDos.Select(ToInnerDto).ToList(),
            CreatedOn = model.CreatedOn,
        };

        public static UpdateToDoDto ToUpdateDto(ToDoModel model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = model.ToDos.Select(ToInnerDto).ToList(),
            CreatedOn = model.CreatedOn,
        };

        public static CompleteToDoDto ToCompleteDto(CompletedToDoModel model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = model.ToDos.Select(ToInnerDto).ToList(),
            CreatedOn = model.CreatedOn,
            CompletedOn = model.CompletedOn,
        };

        public static List<ToDoDto> ToDto(IEnumerable<ToDoViewModel> models) => models.Select(ToDto).ToList();

        public static ToDoDto ToDto(ToDoViewModel model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = ToInnerDto(model.ToDos),
            CreatedOn = model.CreatedOn,
        };

        public static List<ToDoDto> ToDto(IEnumerable<ToDoModel> models) => models.Select(ToDto).ToList();

        public static ToDoDto ToDto(ToDoModel model) => new()
        {
            Id = model.Id,
            Name = model.Name,
            For = model.For,
            ToDos = ToInnerDto(model.ToDos),
            CreatedOn = model.CreatedOn,
        };
    }
}
