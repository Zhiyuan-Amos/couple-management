using Couple.Client.Adapters;
using Couple.Client.Data;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Image;
using Couple.Shared.Model.Issue;

namespace Couple.Client.Services.Synchronizer;

public class CommandParser
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public ICommand Parse(ChangeDto change, AppDbContext dbContext)
    {
        return change switch
        {
            { Command: Command.Create, ContentType: Entity.Issue }
                => new CreateIssueCommand(dbContext,
                    IssueAdapter.ToModel(JsonSerializer.Deserialize<CreateIssueDto>(change.Content, Options)!)),
            { Command: Command.Update, ContentType: Entity.Issue }
                => new UpdateIssueCommand(dbContext,
                    IssueAdapter.ToModel(JsonSerializer.Deserialize<UpdateIssueDto>(change.Content, Options)!)),
            { Command: Command.Delete, ContentType: Entity.Issue }
                => new DeleteIssueCommand(dbContext,
                    JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            { Command: Command.Complete, ContentType: Entity.Task }
                => new CompleteTaskCommand(dbContext,
                    IssueAdapter.ToCompletedModel(
                        JsonSerializer.Deserialize<CompleteTaskDto>(change.Content, Options)!)),
            { Command: Command.Create, ContentType: Entity.Image }
                => new CreateImageCommand(dbContext,
                    ImageAdapter.ToCreateModel(
                        JsonSerializer.Deserialize<CreateImageDto>(change.Content, Options)!)),
            { Command: Command.Update, ContentType: Entity.Image }
                => new UpdateImageCommand(dbContext,
                    ImageAdapter.ToUpdateModel(
                        JsonSerializer.Deserialize<UpdateImageDto>(change.Content, Options)!)),
            { Command: Command.Delete, ContentType: Entity.Image }
                => new DeleteImageCommand(dbContext,
                    JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            _ => throw new ArgumentOutOfRangeException(nameof(change), change, null)
        };
    }
}