using Couple.Client.Features.Calendar.Models;
using Couple.Shared.Models.Calendar;

namespace Couple.Client.Features.Calendar.Adapter;

public class EventAdapter
{
    public static CreateEventDto ToCreateDto(EventModel model) =>
        new(model.Id, model.Title, model.For,
            model.Start, model.End, model.CreatedOn);

    public static UpdateEventDto ToUpdateDto(EventModel model) =>
        new(model.Id, model.Title, model.For,
            model.Start, model.End, model.CreatedOn);
    
    public static EventModel ToModel(CreateEventDto model) =>
        new(model.Id, model.Title, model.For,
            model.Start, model.End, model.CreatedOn);
}
