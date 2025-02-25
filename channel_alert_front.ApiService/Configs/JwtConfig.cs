namespace channel_alert_front.ApiService.Configs;

public class JwtConfig
{
    public static string Section { get; } = "Jwt";
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int Expires { get; set; } = default;
}
