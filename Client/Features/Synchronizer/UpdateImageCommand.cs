using Couple.Client.Shared.Data;
using Couple.Shared.Models.Image;

namespace Couple.Client.Features.Synchronizer;

public class UpdateImageCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly UpdateImageDto _model;

    public UpdateImageCommand(AppDbContext dbContext, UpdateImageDto model) => (_dbContext, _model) = (dbContext, model);

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
