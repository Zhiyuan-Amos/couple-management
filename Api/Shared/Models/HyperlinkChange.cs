using Couple.Shared.Models;

namespace Couple.Api.Shared.Models;

public class HyperlinkChange : Change
{
    public HyperlinkChange(Guid id,
        Command command,
        string userId,
        DateTime timestamp,
        string contentId,
        string url) : base(id, command, userId, timestamp, contentId) =>
        Url = url;

    // ReSharper disable once UnusedMember.Local
    private HyperlinkChange(Guid id,
        string userId,
        DateTime timestamp,
        string contentId,
        string url) : base(id, userId, timestamp, contentId) =>
        Url = url;

    public string Url { get; init; }
}
