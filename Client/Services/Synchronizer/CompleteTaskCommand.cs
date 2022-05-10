using Couple.Client.Data;
using Couple.Client.Model.Issue;
using Couple.Client.Utility;

namespace Couple.Client.Services.Synchronizer;

public class CompleteTaskCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly CreateCompletedTaskModel _model;

    public CompleteTaskCommand(AppDbContext dbContext, CreateCompletedTaskModel model) =>
        (_dbContext, _model) = (dbContext, model);

    public async Task Execute() =>
        await CompleteTaskHelper.CompleteTaskAsync(_model.IssueId, _model.TaskId, _model.CompletedDate, _dbContext);
}