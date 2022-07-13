namespace Couple.Shared.Models.Change;

public class DeleteChangesDto
{
    public DeleteChangesDto(List<Guid> guids) => Guids = new(guids);

    public List<Guid> Guids { get; }
}
