namespace Couple.Client.Model.Done;

public interface IDone
{
    Guid Id { get; }
    DateOnly DoneDate { get; }

    // Should not be manually set; automatically set by DbContext
    int Order { get; set; }

    // Required by EF Core to retrieve original & current DoneDate, which is not a Property
    string DoneDatePropertyName { get; }
}
