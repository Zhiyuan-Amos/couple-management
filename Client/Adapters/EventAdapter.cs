using Couple.Client.Model.Calendar;
using Couple.Client.Model.Issue;
using Couple.Client.ViewModel.Calendar;
using Couple.Shared.Model.Event;
using Couple.Shared.Model.Issue;

namespace Couple.Client.Adapters;

public static class EventAdapter
{
    public static List<EventViewModel> ToViewModel(IEnumerable<EventModel> models)
    {
        return models.Select(ToViewModel).ToList();
    }

    public static EventViewModel ToViewModel(EventModel model)
    {
        return new(model.Id, model.Title, model.Start, model.End, new(model.ToDos));
    }

    public static EventDto ToDto(EventModel model)
    {
        return new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDto(model.ToDos)
        };
    }

    public static EventDto ToDto(UpdateEventViewModel model)
    {
        return new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDto(model.ToDos)
        };
    }

    public static List<IssueDto> ToDto(IEnumerable<IssueModel> models)
    {
        return models.Select(ToDto).ToList();
    }

    public static IssueDto ToDto(IssueModel model)
    {
        return new()
        {
            Id = model.Id,
            Title = model.Title,
            For = model.For,
            Tasks = ToTaskDto(model.Tasks),
            CreatedOn = model.CreatedOn
        };
    }

    public static List<TaskDto> ToTaskDto(IEnumerable<TaskModel> models)
    {
        return models.Select(ToTaskDto).ToList();
    }

    public static TaskDto ToTaskDto(TaskModel model)
    {
        return new(model.Id, model.Content);
    }

    public static EventModel ToModel(UpdateEventViewModel model)
    {
        return new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = model.ToDos
        };
    }

    public static UpdateEventViewModel ToUpdateViewModel(EventModel model)
    {
        return new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = new(model.ToDos)
        };
    }
}
