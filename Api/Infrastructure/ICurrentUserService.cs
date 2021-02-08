namespace Api.Infrastructure
{
    public interface ICurrentUserService
    {
        string Id { get; }
        string Name { get; }
        string Email { get; }
        string PartnerId { get; }
    }
}
