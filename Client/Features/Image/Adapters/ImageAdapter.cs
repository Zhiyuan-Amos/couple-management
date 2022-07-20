using Couple.Client.Features.Image.Models;
using Couple.Shared.Models.Image;

namespace Couple.Client.Features.Image.Adapters;

public static class ImageAdapter
{
    public static CreateImageDto ToCreateDto(ImageModel model) =>
        new(model.Id, model.TakenOn, model.Data, model.IsFavourite);

    public static UpdateImageDto ToUpdateDto(ImageModel model) =>
        new(model.Id, model.TakenOn, model.Data, model.IsFavourite);

    public static ImageModel ToCreateModel(CreateImageDto model) =>
        new(model.Id, model.TakenOn, model.Data, model.IsFavourite);
}
