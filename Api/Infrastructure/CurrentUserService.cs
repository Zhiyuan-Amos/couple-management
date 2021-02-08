using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Api.Infrastructure
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string ClaimTypePartnerId = "PartnerId";
        private readonly ClaimsPrincipal _claimsPrincipal;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _claimsPrincipal = StaticWebAppsAuth.Parse(httpContextAccessor.HttpContext?.Request);
        }

        public string Id => _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Name => _claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        public string Email => _claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        public string PartnerId => _claimsPrincipal.FindFirstValue(ClaimTypePartnerId);

        // from https://docs.microsoft.com/en-us/azure/static-web-apps/user-information?tabs=csharp#api-functions
        public static class StaticWebAppsAuth
        {
            private class ClientPrincipal
            {
                public string IdentityProvider { get; set; }
                public string UserId { get; set; }
                public string UserDetails { get; set; }
                public IEnumerable<string> UserRoles { get; set; }
            }

            public static ClaimsPrincipal Parse(HttpRequest req)
            {
                var data = req.Headers["x-ms-client-principal"][0];
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.ASCII.GetString(decoded);
                var principal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var nameList = principal.UserRoles
                    .Single(role => role.StartsWith("name_"))[5..] // e.g. yong_zhi_yuan
                    .Split('_')
                    .Select(word => word[0].ToString().ToUpper() + word[1..]); // e.g. Yong, Zhi, Yuan
                var name = string.Join(" ", nameList); // e.g. Yong Zhi Yuan

                var partnerId = principal.UserRoles
                    .SingleOrDefault(role => role.StartsWith("partnerId_"));

                principal.UserRoles = principal.UserRoles
                    .Where(role => role != "anonymous"
                        && role != "authenticated"
                        && !role.StartsWith("name_")
                        && !role.StartsWith("partnerId_"))
                    .ToList();

                if (!principal.UserRoles.Any())
                {
                    return new ClaimsPrincipal();
                }

                var identity = new ClaimsIdentity(principal.IdentityProvider);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
                identity.AddClaim(new Claim(ClaimTypes.Name, name));
                identity.AddClaim(new Claim(ClaimTypes.Email, principal.UserDetails));

                if (partnerId != null)
                {
                    identity.AddClaim(new Claim(ClaimTypePartnerId, partnerId[10..]));
                }

                identity.AddClaims(principal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));
                return new ClaimsPrincipal(identity);
            }
        }
    }
}
