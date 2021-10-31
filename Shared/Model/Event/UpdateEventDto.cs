using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Event
{
    public class UpdateEventDto
    {
        public EventDto Event { get; set; }
        public List<Guid> Added { get; set; }
        public List<IssueDto> Removed { get; set; }
    }
}
