using System;

namespace Couple.Api.Model
{
    public class Change
    {
        public Guid Id { get; set; }
        public string Command { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }

        // Ideally, this is a parent type where different forms of content inherit this parent type.
        // However, owned types do not support inheritance https://github.com/dotnet/efcore/issues/9630.
        public string Content { get; set; }
    }
}
