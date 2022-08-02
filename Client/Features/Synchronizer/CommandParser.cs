using System.Text.Json;
using Couple.Client.Features.Calendar.Adapter;
using Couple.Client.Features.Image.Adapters;
using Couple.Client.Features.Issue.Adapters;
using Couple.Client.Shared.Data;
using Couple.Shared.Models;
using Couple.Shared.Models.Calendar;
using Couple.Shared.Models.Change;
using Couple.Shared.Models.Image;
using Couple.Shared.Models.Issue;

namespace Couple.Client.Features.Synchronizer;

public class CommandParser
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public ICommand Parse(ChangeDto change, AppDbContext dbContext) =>
        change.Command switch
        {
            { } command when command.Equals(Command.CreateIssue) => new CreateIssueCommand(dbContext,
                IssueAdapter.ToModel(JsonSerializer.Deserialize<CreateIssueDto>(change.Content, Options)!)),
            { } command when command.Equals(Command.UpdateIssue) => new UpdateIssueCommand(dbContext,
                JsonSerializer.Deserialize<UpdateIssueDto>(change.Content, Options)!),
            { } command when command.Equals(Command.DeleteIssue) => new DeleteIssueCommand(dbContext,
                JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            { } command when command.Equals(Command.CompleteTask) => new CompleteTaskCommand(dbContext,
                IssueAdapter.ToCompletedModel(JsonSerializer.Deserialize<CompleteTaskDto>(change.Content, Options)!)),
            { } command when command.Equals(Command.CreateImage) => new CreateImageCommand(dbContext,
                ImageAdapter.ToCreateModel(JsonSerializer.Deserialize<CreateImageDto>(change.Content, Options)!)),
            { } command when command.Equals(Command.UpdateImage) => new UpdateImageCommand(dbContext,
                JsonSerializer.Deserialize<UpdateImageDto>(change.Content, Options)!),
            { } command when command.Equals(Command.DeleteImage) => new DeleteImageCommand(dbContext,
                JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            { } command when command.Equals(Command.CreateEvent) => new CreateEventCommand(dbContext,
                EventAdapter.ToModel(JsonSerializer.Deserialize<CreateEventDto>(change.Content, Options)!)),
            { } command when command.Equals(Command.UpdateEvent) => new UpdateEventCommand(dbContext,
                JsonSerializer.Deserialize<UpdateEventDto>(change.Content, Options)!),
            { } command when command.Equals(Command.DeleteEvent) => new DeleteEventCommand(dbContext,
                JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            _ => throw new ArgumentOutOfRangeException(nameof(change), change, null)
        };
}
