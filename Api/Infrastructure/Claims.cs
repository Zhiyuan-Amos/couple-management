namespace Couple.Api.Infrastructure;

public class Claims
{
    public Claims(string id, string email, string partnerId)
    {
        Id = id;
        Email = email;
        PartnerId = partnerId;
    }

    public string? Id { get; }
    public string? Email { get; }
    public string? PartnerId { get; }
}
