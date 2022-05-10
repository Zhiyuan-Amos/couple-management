using Couple.Client.Data;

namespace Couple.Client.Services.Synchronizer;

public class DeleteIssueCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly Guid _guid;

    public DeleteIssueCommand(AppDbContext dbContext, Guid guid) => (_dbContext, _guid) = (dbContext, guid);

    public async Task Execute()
    {
        var toDelete = (await _dbContext.Issues.FindAsync(_guid))!;
        _dbContext.Issues.Remove(toDelete);
        await _dbContext.SaveChangesAsync();
    }
}
