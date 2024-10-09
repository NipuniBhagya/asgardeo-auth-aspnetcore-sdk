using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Asgardeo.AspNetCore.Authenthentication.Configuration;

public class AsgardeoAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AsgardeoOptions _options;

    public AsgardeoAuthService(IHttpContextAccessor httpContextAccessor, IOptions<AsgardeoOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    public async Task SignInAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = _options.RedirectUri
            });
        }
    }

    public async Task SignOutAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = _options.PostLogoutRedirectUri
            });
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    public ClaimsPrincipal GetUser()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.User;
    }

    public string GetAccessToken()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.GetTokenAsync("access_token").Result;
    }

    public string GetIdToken()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.GetTokenAsync("id_token").Result;
    }

    public string GetUserInfo()
    {
        var user = GetUser();
        if (user == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var userInfo = new
        {
            Name = user.FindFirst(ClaimTypes.Name)?.Value,
            Email = user.FindFirst(ClaimTypes.Email)?.Value,
            Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        return System.Text.Json.JsonSerializer.Serialize(userInfo);
    }
}
