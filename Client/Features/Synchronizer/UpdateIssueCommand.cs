using Couple.Client.Features.Issue.Models;
using Couple.Client.Shared.Data;

namespace Couple.Client.Features.Synchronizer;

public class UpdateIssueCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly IssueModel _model;

    public UpdateIssueCommand(AppDbContext dbContext, IssueModel model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        var issue = (await _dbContext.Issues.FindAsync(_model.Id))!;
        _dbContext.Attach(issue);
        issue.Title = _model.Title;
        issue.For = _model.For;
        issue.Tasks = _model.Tasks;
        await _dbContext.SaveChangesAsync();
    }
}
