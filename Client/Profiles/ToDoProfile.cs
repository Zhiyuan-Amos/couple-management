using AutoMapper;
using Couple.Client.Model.ToDo;
using Couple.Client.ViewModel.ToDo;
using Couple.Shared.Model.ToDo;

namespace Couple.Client.Profiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<ToDoModel, ToDoViewModel>().ReverseMap();
            CreateMap<ToDoModel, CreateToDoDto>();
            CreateMap<ToDoModel, UpdateToDoDto>();
            CreateMap<ToDoInnerModel, ToDoInnerViewModel>().ReverseMap();
            CreateMap<ToDoInnerModel, CreateUpdateInnerViewModel>().ReverseMap();
            CreateMap<ToDoInnerModel, ToDoInnerDto>().ReverseMap();
        }
    }
}
