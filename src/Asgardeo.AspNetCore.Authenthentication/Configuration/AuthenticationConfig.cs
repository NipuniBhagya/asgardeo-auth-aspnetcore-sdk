using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;

namespace Asgardeo.AspNetCore.Authenthentication.Configuration
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://api.asgardeo.io/t/nipunib/oauth2/token";
                options.ClientId = "wdufxgtTvyRsbiZ2QLFClxzkmhMa";
                options.ClientSecret = "ocf9KMh3qfQAVKF0FswKQem43vcmFTCVRLUs74H7u1Qa";
                options.ResponseType = "code";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.SaveTokens = true;
                options.CallbackPath = "/signin-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";
                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "roles";
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    options.Authority + "/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever());

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // Ensure clock skew is accounted for
                    ClockSkew = TimeSpan.FromMinutes(5),
                    SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                    {
                        var jwt = new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token);

                        return jwt;
                    }
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri = $"{options.Authority}/oidc/logout";
                        var postLogoutUri = $"{context.Request.Scheme}://{context.Request.Host}{options.SignedOutCallbackPath}";

                        context.ProtocolMessage.IssuerAddress = logoutUri;
                        context.ProtocolMessage.PostLogoutRedirectUri = postLogoutUri;

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
