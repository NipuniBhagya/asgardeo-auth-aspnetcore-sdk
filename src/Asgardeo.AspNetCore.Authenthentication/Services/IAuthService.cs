namespace Asgardeo.AspNetCore.Authenthentication.Services
{
    public interface IAuthService
    {
        Task Login();
        Task Logout();
        Task<string> GetAccessToken();
        Task<bool> IsAuthenticated();
    }
}
