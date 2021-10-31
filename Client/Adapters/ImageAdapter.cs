using Couple.Client.Model.Image;
using Couple.Shared.Model.Image;

namespace Couple.Client.Adapters
{
    public static class ImageAdapter
    {
        public static CreateImageDto ToCreateDto(CreateImageModel model) => new()
        {
            Id = model.Id,
            TakenOn = model.TakenOn,
            Data = model.Data,
            IsFavourite = model.IsFavourite,
        };

        public static UpdateImageDto ToUpdateDto(UpdateImageModel model) => new()
        {
            Id = model.Id,
            TakenOn = model.TakenOn,
            Data = model.Data,
            IsFavourite = model.IsFavourite,
        };

        public static CreateImageModel ToCreateModel(CreateImageDto model) =>
            new(model.Id, model.TakenOn) { Data = model.Data, IsFavourite = model.IsFavourite };

        public static UpdateImageModel ToUpdateModel(UpdateImageDto model) =>
            new(model.Id, model.TakenOn, model.Data, model.IsFavourite);
    }
}
