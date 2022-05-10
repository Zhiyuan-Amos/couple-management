namespace Couple.Api.Model;

public class CachedChange : Change
{
    public CachedChange(Guid id,
        string command,
        string userId,
        DateTime timestamp,
        string contentId,
        string contentType,
        string content) : base(id, command, userId, timestamp, contentId, contentType) =>
        Content = content;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Content { get; init; }
}