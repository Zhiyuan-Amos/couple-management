using System;

namespace Couple.Client.ViewModel.ToDo
{
    public class UpdateToDoViewModel
    {
        public Guid Id { get; init; }
        public string Text { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; init; }
    }
}
