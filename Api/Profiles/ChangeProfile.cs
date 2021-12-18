using AutoMapper;
using Couple.Api.Model;
using Couple.Shared.Model.Change;

namespace Couple.Api.Profiles;

public class ChangeProfile : Profile
{
    public ChangeProfile() => CreateMap<Change, ChangeDto>();
}
