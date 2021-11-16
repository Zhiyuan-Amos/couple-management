using System;

namespace Couple.Shared.Model.Change
{
    public class ChangeDto
    {
        public Guid Id { get; }
        public string Command { get; }
        public string Content { get; }

        public ChangeDto(Guid id, string command, string content)
        {
            Id = id;
            Command = command;
            Content = content;
        }
    }
}
