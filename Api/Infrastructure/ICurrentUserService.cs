using System.Net.Http.Headers;

namespace Couple.Api.Infrastructure;

public interface ICurrentUserService
{
    Claims GetClaims(HttpHeaders headers);
}
