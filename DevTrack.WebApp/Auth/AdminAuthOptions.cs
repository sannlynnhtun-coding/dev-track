namespace DevTrack.WebApp.Auth;

public class AdminAuthOptions
{
    public const string SectionName = "AdminAuth";

    public string Username
    {
        get;
        set => field = value?.Trim() ?? string.Empty;
    } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string DisplayName
    {
        get;
        set => field = value?.Trim() ?? string.Empty;
    } = "System Admin";

    public bool IsValid()
        => !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(Password)
            && !string.IsNullOrWhiteSpace(DisplayName);
}
