namespace Couple.Shared.Model.Change;

public class DeleteChangesDto
{
    public DeleteChangesDto(List<Guid> guids) => Guids = new(guids);

    public List<Guid> Guids { get; }
}
