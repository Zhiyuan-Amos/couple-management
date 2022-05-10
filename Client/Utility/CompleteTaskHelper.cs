using Couple.Client.Data;
using Couple.Client.Model.Done;
using Couple.Client.Model.Issue;

namespace Couple.Client.Utility;

public static class CompleteTaskHelper
{
    public static async Task CompleteTaskAsync(Guid issueId, Guid taskId, DateOnly doneDate, AppDbContext dbContext)
    {
        var issue = await dbContext.Issues.FindAsync(issueId);
        await CompleteTaskAsync(issue!, taskId, doneDate, dbContext);
    }

    public static async Task<DoneIssueModel> CompleteTaskAsync(IssueModel issue,
        Guid taskId,
        DateOnly doneDate,
        AppDbContext dbContext)
    {
        dbContext.Attach(issue);

        var completedTask = issue.Tasks.First(t => t.Id == taskId);
        issue.Tasks = issue.Tasks
            .Where(t => t.Id != taskId)
            .ToList();

        var doneIssue = await dbContext.DoneIssues
            .AsTracking()
            .FirstOrDefaultAsync(di => di.DoneDate == doneDate && di.Title == issue.Title);

        if (doneIssue == null)
        {
            var doneTask = new List<DoneTaskModel> { new(completedTask.Content) };
            doneIssue = new(doneDate, doneTask, issue.For, issue.Title);
            dbContext.DoneIssues.Add(doneIssue);
        }
        else
        {
            doneIssue.Tasks.Add(new(completedTask.Content));
        }

        await dbContext.SaveChangesAsync();
        return doneIssue;
    }
}