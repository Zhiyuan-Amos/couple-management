using Couple.Client.Model.Image;
using Couple.Shared.Model.Image;

namespace Couple.Client.Adapters
{
    public static class ImageAdapter
    {
        public static CreateImageDto ToDto(CreateImageModel model) => new()
        {
            Id = model.Id,
            TakenOn = model.TakenOn,
            Data = model.Data,
        };

        public static CreateImageModel ToModel(CreateImageDto model) => new(model.Id, model.TakenOn, model.Data);
    }
}
