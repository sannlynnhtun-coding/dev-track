namespace DevTrack.WebApp.Auth;

public interface IAdminJwtTokenService
{
    string CreateAdminToken(string username, string displayName);
}
