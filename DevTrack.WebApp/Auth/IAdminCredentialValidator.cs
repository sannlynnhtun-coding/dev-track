namespace DevTrack.WebApp.Auth;

public interface IAdminCredentialValidator
{
    bool Validate(string username, string password);
}
