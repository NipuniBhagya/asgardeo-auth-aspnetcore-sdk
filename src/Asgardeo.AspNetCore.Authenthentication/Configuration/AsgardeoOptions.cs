namespace Asgardeo.AspNetCore.Authenthentication.Configuration
{
    public class AsgardeoOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string ResponseType { get; set; } = "code";
        public string[] Scopes { get; set; } = [ "openid", "profile", "email" ];
    }
}
