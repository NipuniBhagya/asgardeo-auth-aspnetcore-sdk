using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            _navigationManager.NavigateTo("account/login", forceLoad: true);
        }

        public async Task Logout()
        {
            _navigationManager.NavigateTo("account/logout", forceLoad: true);
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
