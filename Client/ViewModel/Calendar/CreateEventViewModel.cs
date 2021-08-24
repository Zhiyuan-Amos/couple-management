using Couple.Client.ViewModel.Issue;
using System;
using System.Collections.Generic;

namespace Couple.Client.ViewModel.Calendar
{
    public class CreateEventViewModel
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<IssueViewModel> ToDos { get; set; }
    }
}
