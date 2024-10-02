using AsgardeoBlazorServerSample.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

using Asgardeo.AspNetCore.Authenthentication.Configuration;
using Asgardeo.AspNetCore.Authenthentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddScoped<IAuthService, AsgardeoAuthService>();
builder.Services.AddScoped<UserState>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/account/login", (HttpContext context) =>
{
    return Results.Challenge(new AuthenticationProperties { RedirectUri = "/home" }, 
        new[] { OpenIdConnectDefaults.AuthenticationScheme });
});

app.MapGet("/account/logout", async (HttpContext context) =>
{
    // Sign out locally (from the cookie and OpenID Connect authentication schemes)
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

    // Redirect the user to Asgardeo's logout endpoint
    var logoutUrl = "https://api.asgardeo.io/t/nipunib/oidc/logout";
    var postLogoutRedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/login"; // Redirect back to /login after logout

    // Construct the full logout URL with post_logout_redirect_uri
    var redirectUrl = $"{logoutUrl}?post_logout_redirect_uri={Uri.EscapeDataString(postLogoutRedirectUri)}";

    // Redirect the user to the Asgardeo logout page
    return Results.Redirect(redirectUrl);
});

app.Run();