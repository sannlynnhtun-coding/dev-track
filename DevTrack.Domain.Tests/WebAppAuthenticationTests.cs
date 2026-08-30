using System.Net;
using System.Text.RegularExpressions;
using DevTrack.WebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace DevTrack.Domain.Tests;

public class WebAppAuthenticationTests
{
    [Fact]
    public async Task Home_RedirectsToLogin_WhenUnauthenticated()
    {
        await using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.EndsWith("/Account/Login?ReturnUrl=%2F", response.Headers.Location?.OriginalString);
        Assert.Contains("ReturnUrl=%2F", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task Login_RedirectsToReturnUrl_WhenCredentialsAreValid()
    {
        await using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var loginPage = await client.GetAsync("/Account/Login?returnUrl=%2FBatches");
        var token = ExtractRequestVerificationToken(await loginPage.Content.ReadAsStringAsync());

        var response = await client.PostAsync("/Account/Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Username"] = AuthTestSettings.Username,
            ["Password"] = AuthTestSettings.Password,
            ["ReturnUrl"] = "/Batches",
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/Batches", response.Headers.Location?.OriginalString);
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var cookies));
        Assert.Contains(cookies, cookie => cookie.Contains(".AspNetCore.Cookies"));
    }

    [Fact]
    public async Task Logout_ClearsAuthenticationCookie()
    {
        await using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        await LoginAsync(client);
        var deniedPage = await client.GetAsync("/Account/Denied");
        var token = ExtractRequestVerificationToken(await deniedPage.Content.ReadAsStringAsync());

        var response = await client.PostAsync("/Account/Logout", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/Account/Login", response.Headers.Location?.OriginalString);
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var cookies));
        Assert.Contains(cookies, cookie => cookie.Contains(".AspNetCore.Cookies") && cookie.Contains("expires=Thu, 01 Jan 1970"));
    }

    private static WebApplicationFactory<WebAppAssemblyMarker> CreateFactory()
        => new WebApplicationFactory<WebAppAssemblyMarker>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(AuthTestSettings.WebAppConfiguration);
                });
            });

    private static async Task LoginAsync(HttpClient client)
    {
        var loginPage = await client.GetAsync("/Account/Login");
        var token = ExtractRequestVerificationToken(await loginPage.Content.ReadAsStringAsync());

        var response = await client.PostAsync("/Account/Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Username"] = AuthTestSettings.Username,
            ["Password"] = AuthTestSettings.Password,
            ["ReturnUrl"] = "/",
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    private static string ExtractRequestVerificationToken(string html)
    {
        var match = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"", RegexOptions.IgnoreCase);
        Assert.True(match.Success, "The antiforgery token was not found in the rendered form.");
        return WebUtility.HtmlDecode(match.Groups[1].Value);
    }
}
