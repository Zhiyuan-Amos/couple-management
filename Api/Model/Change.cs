using System;

namespace Couple.Api.Model
{
    public class Change
    {
        public Guid Id { get; init; }
        public string Command { get; init; }
        public string UserId { get; init; }
        public DateTime Timestamp { get; init; }
        public string Content { get; init; }

        private Change() { }

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
