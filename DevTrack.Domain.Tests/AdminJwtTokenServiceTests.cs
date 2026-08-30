using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevTrack.Shared.Security;
using DevTrack.WebApp.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevTrack.Domain.Tests;

public class AdminJwtTokenServiceTests
{
    [Fact]
    public void CreateAdminToken_IncludesAdminIdentityAndValidatesWithSigningKey()
    {
        var service = new AdminJwtTokenService(Options.Create(new JwtOptions
        {
            Issuer = AuthTestSettings.Issuer,
            Audience = AuthTestSettings.Audience,
            SigningKey = AuthTestSettings.SigningKey,
            ExpiresMinutes = 480
        }));

        var token = service.CreateAdminToken(AuthTestSettings.Username, AuthTestSettings.DisplayName);

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthTestSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = AuthTestSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthTestSettings.SigningKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        Assert.IsType<JwtSecurityToken>(validatedToken);
        Assert.Equal(AuthTestSettings.DisplayName, principal.Identity?.Name);
        Assert.True(principal.IsInRole(AuthConstants.AdminRole));
        Assert.Equal(AuthTestSettings.Username, principal.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
