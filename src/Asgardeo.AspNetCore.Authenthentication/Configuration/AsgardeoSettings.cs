namespace Asgardeo.AspNetCore.Authenthentication.Configuration
{
    public class AsgardeoSettings
    {
        public string Authority { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string ResponseType { get; set; } = "code";
        public List<string> Scopes { get; set; } = new List<string>();
        public bool SaveTokens { get; set; } = true;
    }
}
