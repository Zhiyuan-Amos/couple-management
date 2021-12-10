namespace Couple.Shared.Model.Change;

public class DeleteChangeDto
{
    public List<Guid> Guids { get; }

    public DeleteChangeDto(List<Guid> guids)
    {
        Guids = new(guids);
    }
}
