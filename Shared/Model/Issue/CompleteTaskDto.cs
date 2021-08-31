using System;

namespace Couple.Shared.Model.Issue
{
    public class CompleteTaskDto
    {
        public Guid Id { get; set; }
        public For For { get; set; }
        public string Content { get; set; }

        public Guid IssueId { get; set; }
        public string IssueTitle { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
