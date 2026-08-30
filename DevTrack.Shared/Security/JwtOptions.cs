namespace DevTrack.Shared.Security;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer
    {
        get;
        set => field = value?.Trim() ?? string.Empty;
    } = string.Empty;

    public string Audience
    {
        get;
        set => field = value?.Trim() ?? string.Empty;
    } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int ExpiresMinutes { get; set; } = 480;

    public bool IsValid()
        => !string.IsNullOrWhiteSpace(Issuer)
            && !string.IsNullOrWhiteSpace(Audience)
            && !string.IsNullOrWhiteSpace(SigningKey)
            && SigningKey.Length >= 32
            && ExpiresMinutes > 0;
}
