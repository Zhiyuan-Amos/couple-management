namespace Couple.Client.Model.Done;

public interface IDone
{
    DateOnly DoneDate { get; }

    // Should not be manually set; automatically set by DbContext
    int Order { get; set; }
}
