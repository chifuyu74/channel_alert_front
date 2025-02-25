using System.Text.Json.Serialization;

namespace channel_alert_front.Shared.Models;

[Serializable]
public class LoginRequestModel
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Email: {Email}, Password: {Password}";
    }
}

[Serializable, JsonSerializable(typeof(LoginResponseModel))]
public class LoginResponseModel
{
    [JsonPropertyName("access")]
    public string Access { get; set; } = string.Empty;
    [JsonPropertyName("refresh")]
    public string Refresh { get; set; } = string.Empty;
}

public class RefreshTokenModel
{
    [JsonPropertyName("access")]
    public string? AccessToken { get; set; }
    [JsonPropertyName("refresh")]
    public string? RefreshToken { get; set; }
}