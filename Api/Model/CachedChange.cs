using System;

namespace Couple.Api.Model
{
    public class CachedChange : Change
    {
        private CachedChange() { }

        public CachedChange(Guid id,
            string command,
            string userId,
            DateTime timestamp,
            Guid contentId,
            string contentType,
            string content) : base(id, command, userId, timestamp, contentId, contentType, content) { }
    }
}
