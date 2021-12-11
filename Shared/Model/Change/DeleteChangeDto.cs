namespace Couple.Shared.Model.Change;

public class DeleteChangeDto
{
    public DeleteChangeDto(List<Guid> guids)
    {
        Guids = new(guids);
    }

    public List<Guid> Guids { get; }
}
