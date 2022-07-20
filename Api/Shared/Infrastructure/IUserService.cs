namespace Couple.Api.Shared.Infrastructure;

public interface IUserService
{
    Claims GetClaims(IHeaderDictionary headers);
}
