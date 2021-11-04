using Couple.Client.Model.Issue;
using System;
using System.Collections.Generic;

namespace Couple.Client.ViewModel.Calendar
{
    public class UpdateEventViewModel
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<IssueModel> ToDos { get; set; }
    }
}
