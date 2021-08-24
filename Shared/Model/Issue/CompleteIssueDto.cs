using System;

namespace Couple.Shared.Model.Issue
{
    public class CompleteIssueDto : CreateIssueDto
    {
        public DateTime CompletedOn { get; set; }
    }
}
