using Couple.Client.Features.Issue.Models;
using Couple.Client.Shared.Data;

namespace Couple.Client.Features.Synchronizer;

public class CreateIssueCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly IssueModel _model;

    public CreateIssueCommand(AppDbContext dbContext, IssueModel model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        _dbContext.Issues.Add(_model);
        await _dbContext.SaveChangesAsync();
    }
}
