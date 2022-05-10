namespace Couple.Client.Services.Settings;

public class AuthenticationOptions
{
    public string Authority { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string KnownAuthority { get; set; } = default!;
    public string RedirectUri { get; set; } = default!;
    public string PostLogoutRedirectUri { get; set; } = default!;
}
