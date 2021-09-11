using Couple.Client.Model.Image;
using Couple.Shared.Model.Image;

namespace Couple.Client.Adapters
{
    public static class ImageAdapter
    {
        public static CreateImageDto ToDto(ImageModel model) => new()
        {
            Id = model.Id,
            TakenOn = model.TakenOn,
        };
    }
}
