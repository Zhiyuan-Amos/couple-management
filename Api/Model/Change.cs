using System;

namespace Couple.Api.Model
{
    public class Change
    {
        public Guid Id { get; }
        public string Command { get; }
        public string UserId { get; }
        public DateTime Timestamp { get; }
        public string Content { get; }

        public Change(Guid id, string command, string userId, DateTime timestamp, string content)
        {
            Id = id;
            Command = command;
            UserId = userId;
            Timestamp = timestamp;
            Content = content;
        }
    }
}
