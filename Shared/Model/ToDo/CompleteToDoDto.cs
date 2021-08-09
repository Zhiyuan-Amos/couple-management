using System;

namespace Couple.Shared.Model.ToDo
{
    public class CompleteToDoDto : CreateToDoDto
    {
        public DateTime CompletedOn { get; set; }
    }
}