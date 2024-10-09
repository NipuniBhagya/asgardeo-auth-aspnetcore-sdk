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
using Microsoft.Extensions.Options;

namespace Asgardeo.AspNetCore.Authenthentication.Configuration
{
    public static class AsgardeoExtensions
    {
        public static IServiceCollection AddAsgardeoAuthentication(this IServiceCollection services, Action<AsgardeoOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddHttpContextAccessor();
            services.AddTransient<AsgardeoAuthService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var asgardeoOptions = serviceProvider.GetRequiredService<IOptions<AsgardeoOptions>>().Value;

                // Use values from the configuration model
                options.Authority = asgardeoOptions.Authority;
                options.ClientId = asgardeoOptions.ClientId;
                options.ClientSecret = asgardeoOptions.ClientSecret;
                options.ResponseType = asgardeoOptions.ResponseType;
                options.SaveTokens = true;

                // Add scopes
                foreach (var scope in asgardeoOptions.Scopes)
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
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/");
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
