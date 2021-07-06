﻿using Couple.Client.Model.Calendar;
using Couple.Client.ViewModel.Calendar;
using Couple.Shared.Model.Event;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.Adapters
{
    public static class EventAdapter
    {
        public static List<EventViewModel> ToViewModel(IEnumerable<EventModel> models) =>
            models.Select(ToViewModel).ToList();

        public static EventViewModel ToViewModel(EventModel model) =>
            new(model.Id, model.Title, model.Start, model.End, ToDoAdapter.ToViewModel(model.ToDos));

        public static EventDto ToDto(EventModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDoAdapter.ToDto(model.ToDos),
        };

        public static EventDto ToDto(UpdateEventViewModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDoAdapter.ToDto(model.ToDos),
        };

        public static EventModel ToModel(EventDto model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDoAdapter.ToModel(model.ToDos),
        };

        public static EventModel ToModel(UpdateEventViewModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDoAdapter.ToModel(model.ToDos),
        };

        public static UpdateEventViewModel ToUpdateViewModel(EventModel model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Start = model.Start,
            End = model.End,
            ToDos = ToDoAdapter.ToViewModel(model.ToDos),
        };
    }
}
