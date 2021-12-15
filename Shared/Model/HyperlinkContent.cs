namespace Couple.Shared.Model;

public class HyperlinkContent
{
    public HyperlinkContent(string contentId, object content)
    {
        ContentId = contentId;
        Content = content;
    }

    public string ContentId { get; }
    public object Content { get; }
}
