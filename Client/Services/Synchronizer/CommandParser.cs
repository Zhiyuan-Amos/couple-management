using Couple.Client.Adapters;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Image;
using Couple.Shared.Model.Issue;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Couple.Client.Services.Synchronizer;

public class CommandParser
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };
    private readonly IJSRuntime _js;

    public CommandParser(IJSRuntime js)
    {
        _js = js;
    }

    public ICommand Parse(ChangeDto change)
    {
        return change switch
        {
            { Command: Command.Create, ContentType: Entity.Issue }
                => new CreateIssueCommand(_js,
                    IssueAdapter.ToModel(JsonSerializer.Deserialize<CreateIssueDto>(change.Content, Options))),
            { Command: Command.Update, ContentType: Entity.Issue }
                => new UpdateIssueCommand(_js,
                    IssueAdapter.ToModel(JsonSerializer.Deserialize<UpdateIssueDto>(change.Content, Options))),
            { Command: Command.Delete, ContentType: Entity.Issue }
                => new DeleteIssueCommand(_js,
                    JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            { Command: Command.Complete, ContentType: Entity.Task }
                => new CompleteTaskCommand(_js,
                    IssueAdapter.ToCompletedModel(
                        JsonSerializer.Deserialize<CompleteTaskDto>(change.Content, Options))),
            { Command: Command.Create, ContentType: Entity.Image }
                => new CreateImageCommand(_js,
                    ImageAdapter.ToCreateModel(
                        JsonSerializer.Deserialize<CreateImageDto>(change.Content, Options))),
            { Command: Command.Update, ContentType: Entity.Image }
                => new UpdateImageCommand(_js,
                    ImageAdapter.ToUpdateModel(
                        JsonSerializer.Deserialize<UpdateImageDto>(change.Content, Options))),
            { Command: Command.Delete, ContentType: Entity.Image }
                => new DeleteImageCommand(_js,
                    JsonSerializer.Deserialize<Guid>(change.Content, Options)),
            _ => throw new ArgumentOutOfRangeException(nameof(change), change, null)
        };
    }
}
