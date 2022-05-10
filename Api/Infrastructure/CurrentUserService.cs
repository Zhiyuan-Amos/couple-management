using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Couple.Api.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    public Claims GetClaims(HttpHeaders headers)
    {
        var authValues = headers.GetValues("authorization");
        var authHeader = AuthenticationHeaderValue.Parse(authValues.ToArray().First());
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(authHeader.Parameter);
        return new(jwt.Subject, jwt.Claims.First(c => c.Type == "name").Value);
    }
}