using System;

namespace Couple.Shared.Model
{
    public class ChangeDto
    {
        public Guid Id { get; set; }
        public Function Function { get; set; }
        public DataType DataType { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
    }
}
