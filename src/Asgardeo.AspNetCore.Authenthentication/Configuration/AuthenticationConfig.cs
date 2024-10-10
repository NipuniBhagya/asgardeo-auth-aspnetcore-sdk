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
            var asgardeoSettings = configuration.GetSection("Authentication:Asgardeo").Get<AsgardeoSettings>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // Use values from the configuration model
                options.Authority = asgardeoSettings.Authority;
                options.ClientId = asgardeoSettings.ClientId;
                options.ClientSecret = asgardeoSettings.ClientSecret;
                options.ResponseType = asgardeoSettings.ResponseType;
                options.SaveTokens = asgardeoSettings.SaveTokens;

                // Add scopes
                foreach (var scope in asgardeoSettings.Scopes)
                {
                    options.Scope.Add(scope);
                }
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
