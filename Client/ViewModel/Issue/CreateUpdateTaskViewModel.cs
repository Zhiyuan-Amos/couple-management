using System;

namespace Couple.Client.ViewModel.Issue
{
    public class CreateUpdateTaskViewModel : IReadOnlyTaskViewModel
    {
        public Guid Id { get; }
        public string Content { get; set; }

        public CreateUpdateTaskViewModel(Guid id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
