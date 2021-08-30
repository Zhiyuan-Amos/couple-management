using System;

namespace Couple.Client.ViewModel.Issue
{
    public class TaskViewModel
    {
        public Guid Id { get; init; }
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
    }
}
