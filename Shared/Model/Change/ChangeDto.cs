using System;

namespace Couple.Shared.Model.Change
{
    public class ChangeDto
    {
        public Guid Id { get; set; }
        public string Command { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
    }
}
