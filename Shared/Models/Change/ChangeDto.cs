namespace Couple.Shared.Models.Change;

public class ChangeDto
{
    public ChangeDto(Guid id, Command command, string content)
    {
        Id = id;
        Command = command;
        Content = content;
    }

    public Guid Id { get; }
    public Command Command { get; }
    public string Content { get; }
}
