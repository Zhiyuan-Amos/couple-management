using AutoMapper;
using Couple.Api.Model;
using Couple.Shared.Model.Image;

namespace Couple.Api.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile() {
            CreateMap<CreateImageDto, Image>();
            CreateMap<UpdateImageDto, Image>();
        }
    }
}
