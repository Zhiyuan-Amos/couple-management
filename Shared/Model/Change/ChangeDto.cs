using System;

namespace Couple.Shared.Model.Change
{
    public class ChangeDto
    {
        public Guid Id { get; }
        public string Command { get; }
        public string UserId { get; }
        public DateTime Timestamp { get; }
        public string Content { get; }

        public ChangeDto(Guid id, string command, string userId, DateTime timestamp, string content)
        {
            Id = id;
            Command = command;
            UserId = userId;
            Timestamp = timestamp;
            Content = content;
        }
    }
}
