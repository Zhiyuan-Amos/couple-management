namespace Couple.Api.Shared.Infrastructure;

public class Claims
{
    public Claims(string id, string partnerId)
    {
        Id = id;
        PartnerId = partnerId;
    }

    public string Id { get; }
    public string PartnerId { get; }
}
