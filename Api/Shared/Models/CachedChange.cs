using Couple.Shared.Models;

namespace Couple.Api.Shared.Models;

public class CachedChange : Change
{
    public CachedChange(Guid id,
        Command command,
        string userId,
        DateTime timestamp,
        string contentId,
        string content) : base(id, command, userId, timestamp, contentId) =>
        Content = content;

    // ReSharper disable once UnusedMember.Local
    private CachedChange(Guid id,
        string userId,
        DateTime timestamp,
        string contentId,
        string content) : base(id, userId, timestamp, contentId) =>
        Content = content;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Content { get; init; }
}
