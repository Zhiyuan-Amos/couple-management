using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Event
{
    public class CreateEventDto
    {
        public EventDto Event { get; set; }
        public List<Guid> Added { get; set; }
    }
}
