using System;
using System.Collections.Generic;

namespace Couple.Shared.Model.Issue
{
    public class CreateIssueDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public For For { get; set; }
        public List<TaskDto> Tasks { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
