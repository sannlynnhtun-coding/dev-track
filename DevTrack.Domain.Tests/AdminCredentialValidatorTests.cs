using DevTrack.WebApp.Auth;
using Microsoft.Extensions.Options;

namespace DevTrack.Domain.Tests;

public class AdminCredentialValidatorTests
{
    [Fact]
    public void Validate_ReturnsTrue_ForConfiguredCredentials()
    {
        var validator = CreateValidator();

        var isValid = validator.Validate(AuthTestSettings.Username, AuthTestSettings.Password);

        Assert.True(isValid);
    }

    [Fact]
    public void Validate_ReturnsFalse_ForInvalidUsername()
    {
        var validator = CreateValidator();

        var isValid = validator.Validate("other-admin", AuthTestSettings.Password);

        Assert.False(isValid);
    }

    [Fact]
    public void Validate_ReturnsFalse_ForInvalidPassword()
    {
        var validator = CreateValidator();

        var isValid = validator.Validate(AuthTestSettings.Username, "wrong-password");

        Assert.False(isValid);
    }

    private static AdminCredentialValidator CreateValidator()
        => new(Options.Create(new AdminAuthOptions
        {
            Username = AuthTestSettings.Username,
            Password = AuthTestSettings.Password,
            DisplayName = AuthTestSettings.DisplayName
        }));
}
