using System;
using System.Collections.Generic;
using Couple.Shared.Model.Issue;

namespace Couple.Shared.Model.Event
{
    public class IssueDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public For For { get; set; }
        public List<TaskDto> Tasks { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
