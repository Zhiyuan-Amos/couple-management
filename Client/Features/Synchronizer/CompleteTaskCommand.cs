using Couple.Client.Features.Issue.Models;
using Couple.Client.Shared.Data;
using Couple.Client.Shared.Helpers;

namespace Couple.Client.Features.Synchronizer;

public class CompleteTaskCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly CreateCompletedTaskModel _model;

    public CompleteTaskCommand(AppDbContext dbContext, CreateCompletedTaskModel model) =>
        (_dbContext, _model) = (dbContext, model);

    public async Task Execute() =>
        await CompleteTaskHelper.CompleteTaskAsync(_model.IssueId, _model.TaskId, _model.CompletedDate, _dbContext);
}
