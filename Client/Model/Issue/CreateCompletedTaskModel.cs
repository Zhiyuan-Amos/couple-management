using Couple.Shared.Model;
using System;

namespace Couple.Client.Model.Issue
{
    public class CreateCompletedTaskModel
    {
        public Guid Id { get; init; }
        public For For { get; init; }
        public string Content { get; init; }

        public Guid IssueId { get; init; }
        public string IssueTitle { get; init; }
        public DateTime CreatedOn { get; init; }
        public string CreatedOnDate => CreatedOn.ToString("dd/MM/yyyy");
    }
}
