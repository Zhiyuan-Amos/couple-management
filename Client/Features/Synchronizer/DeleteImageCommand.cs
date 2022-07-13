using Couple.Client.Shared.Data;

namespace Couple.Client.Features.Synchronizer;

public class DeleteImageCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly Guid _guid;

    public DeleteImageCommand(AppDbContext dbContext, Guid guid) => (_dbContext, _guid) = (dbContext, guid);

    public async Task Execute()
    {
        var toDelete = (await _dbContext.Images.FindAsync(_guid))!;
        _dbContext.Images.Remove(toDelete);
        await _dbContext.SaveChangesAsync();
    }
}
