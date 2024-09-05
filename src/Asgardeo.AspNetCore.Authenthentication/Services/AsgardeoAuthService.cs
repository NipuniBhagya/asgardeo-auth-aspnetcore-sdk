using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components;

namespace Asgardeo.AspNetCore.Authenthentication.Services
{
    public class AsgardeoAuthService : IAuthService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AsgardeoAuthService(NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor)
        {
            _navigationManager = navigationManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Login()
        {
            _navigationManager.NavigateTo("authentication/login");
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            _navigationManager.NavigateTo("authentication/logout");
        }

        public async Task<string> GetAccessToken()
        {
            return await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        }

        public async Task<bool> IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
