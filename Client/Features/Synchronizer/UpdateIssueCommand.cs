using Couple.Client.Features.Issue.Adapters;
using Couple.Client.Shared.Data;
using Couple.Shared.Models.Issue;

namespace Couple.Client.Features.Synchronizer;

public class UpdateIssueCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly UpdateIssueDto _model;

    public UpdateIssueCommand(AppDbContext dbContext, UpdateIssueDto model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        var issue = (await _dbContext.Issues.FindAsync(_model.Id))!;
        _dbContext.Attach(issue);
        issue.Title = _model.Title;
        issue.For = _model.For;
        issue.Tasks = IssueAdapter.ToTaskModel(_model.Tasks);
        await _dbContext.SaveChangesAsync();
    }
}
