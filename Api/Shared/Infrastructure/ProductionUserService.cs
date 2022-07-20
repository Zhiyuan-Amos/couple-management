using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Couple.Api.Shared.Infrastructure;

public class ProductionUserService : IUserService
{
    public Claims GetClaims(IHeaderDictionary headers)
    {
        var authValues = headers["authorization"];
        var authHeader = AuthenticationHeaderValue.Parse(authValues.ToArray().First());
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(authHeader.Parameter);
        return new(jwt.Subject, jwt.Claims.First(c => c.Type == "name").Value);
    }
}
