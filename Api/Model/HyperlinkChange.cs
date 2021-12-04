using System;

namespace Couple.Api.Model
{
    public class HyperlinkChange : Change
    {
        public string Url { get; init; }

        private HyperlinkChange() { }

        public HyperlinkChange(Guid id,
            string command,
            string userId,
            DateTime timestamp,
            Guid contentId,
            string contentType,
            string content,
            string url) : base(id, command, userId, timestamp, contentId, contentType, content)
        {
            Url = url;
        }
    }
}
