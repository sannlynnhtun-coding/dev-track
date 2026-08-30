using DevTrack.Shared.Security;
using DevTrack.WebApp.Auth;

namespace DevTrack.Domain.Tests;

internal static class AuthTestSettings
{
    public const string Username = "admin";
    public const string Password = "correct-password";
    public const string DisplayName = "System Admin";
    public const string Issuer = "DevTrack.Tests";
    public const string Audience = "DevTrack.Api.Tests";
    public const string SigningKey = "devtrack-tests-signing-key-with-32-plus-chars";

    public static IReadOnlyDictionary<string, string?> WebAppConfiguration => new Dictionary<string, string?>
    {
        [$"{AdminAuthOptions.SectionName}:Username"] = Username,
        [$"{AdminAuthOptions.SectionName}:Password"] = Password,
        [$"{AdminAuthOptions.SectionName}:DisplayName"] = DisplayName,
        [$"{JwtOptions.SectionName}:Issuer"] = Issuer,
        [$"{JwtOptions.SectionName}:Audience"] = Audience,
        [$"{JwtOptions.SectionName}:SigningKey"] = SigningKey,
        [$"{JwtOptions.SectionName}:ExpiresMinutes"] = "480"
    };

    public static IReadOnlyDictionary<string, string?> ApiConfiguration => new Dictionary<string, string?>
    {
        ["DatabaseProvider"] = "InMemory",
        [$"{JwtOptions.SectionName}:Issuer"] = Issuer,
        [$"{JwtOptions.SectionName}:Audience"] = Audience,
        [$"{JwtOptions.SectionName}:SigningKey"] = SigningKey,
        [$"{JwtOptions.SectionName}:ExpiresMinutes"] = "480"
    };
}
