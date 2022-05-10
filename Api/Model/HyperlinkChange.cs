namespace Couple.Api.Model;

public class HyperlinkChange : Change
{
    public HyperlinkChange(Guid id,
        string command,
        string userId,
        DateTime timestamp,
        string contentId,
        string contentType,
        string url) : base(id, command, userId, timestamp, contentId, contentType)
    {
        Url = url;
    }

    public string Url { get; init; }
}