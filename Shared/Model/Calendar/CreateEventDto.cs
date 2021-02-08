using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Calendar
{
    public class CreateEventDto
    {
        public EventDto Event { get; set; }
        public List<Guid> Added { get; set; }
        public List<ToDoDto> Removed { get; set; }
    }
}