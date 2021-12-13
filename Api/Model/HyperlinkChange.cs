namespace Couple.Api.Model;

public class HyperlinkChange : Change
{
    private HyperlinkChange()
    {
    }

    public HyperlinkChange(Guid id,
        string command,
        string userId,
        DateTime timestamp,
        string contentId,
        string contentType,
        string content,
        string url) : base(id, command, userId, timestamp, contentId, contentType, content)
    {
        Url = url;
    }

    public string Url { get; init; }
}
