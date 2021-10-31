using System;
using System.Collections.Generic;
using Couple.Shared.Model;

namespace Couple.Client.Model.Issue
{
    public class IssueModel
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public For For { get; init; }
        public List<TaskModel> Tasks { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}
