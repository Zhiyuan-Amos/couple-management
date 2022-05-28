using System.Net.Http.Headers;

namespace Couple.Api.Shared.Infrastructure;

public class DevelopmentCurrentUserService : ICurrentUserService
{
    public Claims GetClaims(HttpHeaders headers) => new("Id", "Id");
}
