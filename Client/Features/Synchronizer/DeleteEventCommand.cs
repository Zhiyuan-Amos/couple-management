using Couple.Client.Shared.Data;

namespace Couple.Client.Features.Synchronizer;

public class DeleteEventCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly Guid _guid;

    public DeleteEventCommand(AppDbContext dbContext, Guid guid) => (_dbContext, _guid) = (dbContext, guid);

    public async Task Execute()
    {
        var toDelete = (await _dbContext.Events.FindAsync(_guid))!;
        _dbContext.Events.Remove(toDelete);
        await _dbContext.SaveChangesAsync();
    }
}
