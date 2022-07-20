namespace Couple.Api.Shared.Infrastructure;

public class DevelopmentUserService : IUserService
{
    public Claims GetClaims(IHeaderDictionary headers) => new("Id", "Id");
}
