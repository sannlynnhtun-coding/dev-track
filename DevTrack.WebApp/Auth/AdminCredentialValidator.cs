using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace DevTrack.WebApp.Auth;

public class AdminCredentialValidator : IAdminCredentialValidator
{
    private readonly AdminAuthOptions _options;

    public AdminCredentialValidator(IOptions<AdminAuthOptions> options)
    {
        _options = options.Value;
    }

    public bool Validate(string username, string password)
    {
        if (!_options.IsValid())
        {
            return false;
        }

        return SecureEquals(username, _options.Username)
            && SecureEquals(password, _options.Password);
    }

    private static bool SecureEquals(string left, string right)
    {
        var leftHash = SHA256.HashData(Encoding.UTF8.GetBytes(left));
        var rightHash = SHA256.HashData(Encoding.UTF8.GetBytes(right));
        return CryptographicOperations.FixedTimeEquals(leftHash, rightHash);
    }
}
