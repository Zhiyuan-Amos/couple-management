using Couple.Shared.Models;

namespace Couple.Client.Features.Calendar.Models;

public interface IReadOnlyEventModel
{
    public Guid Id { get; }
    public string Title { get; }
    public For For { get; }
    public DateTime Start { get; }
    public DateTime End { get; }
    public DateTime CreatedOn { get; }
}
