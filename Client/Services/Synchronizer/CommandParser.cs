using System;
using System.Text.Json;
using Couple.Client.Adapters;
using Couple.Shared.Model;
using Couple.Shared.Model.Change;
using Couple.Shared.Model.Image;
using Couple.Shared.Model.Issue;
using Microsoft.JSInterop;

namespace Couple.Client.Services.Synchronizer
{
    public class CommandParser
    {
        private readonly IJSRuntime _js;

        private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

        public CommandParser(IJSRuntime js) => _js = js;

        public ICommand Parse(ChangeDto change)
        {
            return change.Command switch
            {
                Command.CreateIssue => new CreateIssueCommand(_js,
                    IssueAdapter.ToModel(JsonSerializer.Deserialize<CreateIssueDto>(change.Content, Options))),
                Command.UpdateIssue => new UpdateIssueCommand(_js,
                    IssueAdapter.ToModel(JsonSerializer.Deserialize<UpdateIssueDto>(change.Content, Options))),
                Command.DeleteIssue => new DeleteIssueCommand(_js,
                    JsonSerializer.Deserialize<Guid>(change.Content, Options)),
                Command.CompleteTask => new CompleteTaskCommand(_js,
                    IssueAdapter.ToCompletedModel(JsonSerializer.Deserialize<CompleteTaskDto>(change.Content, Options))),
                Command.CreateImage => new CreateImageCommand(_js,
                    ImageAdapter.ToCreateModel(JsonSerializer.Deserialize<CreateImageDto>(change.Content, Options))),
                Command.UpdateImage => new UpdateImageCommand(_js,
                    ImageAdapter.ToUpdateModel(JsonSerializer.Deserialize<UpdateImageDto>(change.Content, Options))),
                Command.DeleteImage => new DeleteImageCommand(_js,
                    JsonSerializer.Deserialize<Guid>(change.Content, Options)),
                _ => throw new ArgumentOutOfRangeException(nameof(change), change, null)
            };
        }
    }
}
