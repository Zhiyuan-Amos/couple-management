using AutoMapper;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.ToDo;
using Couple.Client.ViewModel.Calendar;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model.Event;

namespace Couple.Client.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventModel, EventViewModel>();
            CreateMap<EventModel, EventDto>().ReverseMap();
            CreateMap<ToDoModel, ToDoDto>().ReverseMap();
            CreateMap<ToDoViewModel, ToDoDto>();
            CreateMap<EventModel, UpdateEventViewModel>()
                .ReverseMap();
            CreateMap<UpdateEventViewModel, EventDto>();
        }
    }
}
