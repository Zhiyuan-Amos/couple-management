using System.Net.Http.Headers;

namespace Couple.Api.Infrastructure;

public class DevelopmentCurrentUserService : ICurrentUserService
{
    public Claims GetClaims(HttpHeaders headers) => new("Id", "Id");
}
