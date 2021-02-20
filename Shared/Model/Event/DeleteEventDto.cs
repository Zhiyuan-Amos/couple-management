using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Event
{
    public class DeleteEventDto
    {
        public Guid Id { get; set; }
        public List<ToDoDto> Removed { get; set; }
    }
}