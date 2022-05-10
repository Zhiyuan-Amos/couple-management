using Couple.Client.Model.Image;
using Couple.Shared.Model.Image;

namespace Couple.Client.Adapters;

public static class ImageAdapter
{
    public static CreateImageDto ToCreateDto(ImageModel model)
    {
        return new(model.Id, model.TakenOn, model.Data, model.IsFavourite);
    }

    public static UpdateImageDto ToUpdateDto(ImageModel model)
    {
        return new(model.Id, model.TakenOn, model.Data, model.IsFavourite);
    }

    public static ImageModel ToCreateModel(CreateImageDto model)
    {
        return new(model.Id, model.TakenOn, model.Data, model.IsFavourite);
    }

    public static ImageModel ToUpdateModel(UpdateImageDto model)
    {
        return new(model.Id, model.TakenOn, model.Data, model.IsFavourite);
    }
}