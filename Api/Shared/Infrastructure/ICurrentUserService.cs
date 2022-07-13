using System.Net.Http.Headers;

namespace Couple.Api.Shared.Infrastructure;

public interface ICurrentUserService
{
    Claims GetClaims(HttpHeaders headers);
}
