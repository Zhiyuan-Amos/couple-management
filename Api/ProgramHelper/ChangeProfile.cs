using AutoMapper;
using Couple.Api.Shared.Models;
using Couple.Shared.Models.Change;

namespace Couple.Api.ProgramHelper;

public class ChangeProfile : Profile
{
    public ChangeProfile() => CreateMap<CachedChange, ChangeDto>();
}
