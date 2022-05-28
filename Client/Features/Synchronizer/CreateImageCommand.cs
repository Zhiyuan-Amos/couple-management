using Couple.Client.Features.Image.Models;
using Couple.Client.Shared.Data;

namespace Couple.Client.Features.Synchronizer;

public class CreateImageCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly ImageModel _model;

    public CreateImageCommand(AppDbContext dbContext, ImageModel model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        _dbContext.Images.Add(_model);
        await _dbContext.SaveChangesAsync();
    }
}
