using System.Net;
using System.Net.Http.Headers;
using DevTrack.Api;
using DevTrack.Shared.Security;
using DevTrack.WebApp.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DevTrack.Domain.Tests;

public class ApiAuthenticationTests
{
    [Fact]
    public async Task BatchesEndpoint_ReturnsUnauthorized_WithoutToken()
    {
        await using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/api/batches");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BatchesEndpoint_ReturnsUnauthorized_WithInvalidToken()
    {
        await using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid-token");

        var response = await client.GetAsync("/api/batches");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BatchesEndpoint_ReturnsOk_WithValidAdminToken()
    {
        await using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateAdminToken());

        var response = await client.GetAsync("/api/batches");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static WebApplicationFactory<ApiAssemblyMarker> CreateFactory()
        => new WebApplicationFactory<ApiAssemblyMarker>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(AuthTestSettings.ApiConfiguration);
                });
            });

    private static string CreateAdminToken()
    {
        var service = new AdminJwtTokenService(Options.Create(new JwtOptions
        {
            Issuer = AuthTestSettings.Issuer,
            Audience = AuthTestSettings.Audience,
            SigningKey = AuthTestSettings.SigningKey,
            ExpiresMinutes = 480
        }));

        return service.CreateAdminToken(AuthTestSettings.Username, AuthTestSettings.DisplayName);
    }
}
