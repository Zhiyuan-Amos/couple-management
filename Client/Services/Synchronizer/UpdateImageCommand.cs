using Couple.Client.Data;
using Couple.Client.Model.Image;

namespace Couple.Client.Services.Synchronizer;

public class UpdateImageCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly ImageModel _model;

    public UpdateImageCommand(AppDbContext dbContext, ImageModel model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        var image = (await _dbContext.Images.FindAsync(_model.Id))!;
        _dbContext.Attach(image);
        image.TakenOn = _model.TakenOn;
        image.Data = _model.Data;
        image.IsFavourite = _model.IsFavourite;
        await _dbContext.SaveChangesAsync();
    }
}
