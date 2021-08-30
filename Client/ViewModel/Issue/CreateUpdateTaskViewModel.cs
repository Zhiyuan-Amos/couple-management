using System;

namespace Couple.Client.ViewModel.Issue
{
    public class CreateUpdateTaskViewModel : IReadOnlyTaskViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}
