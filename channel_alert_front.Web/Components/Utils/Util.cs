using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace channel_alert_front.Web.Components.Utils;

public static class Util
{
    // Make JwtService, Inject Service
    public static JwtSecurityToken JwtToToken(string jwt)
    {
        JwtSecurityTokenHandler handler = new();
        if (!handler.CanReadToken(jwt))
            return null;

        SecurityToken token = handler.ReadToken(jwt);
        JwtSecurityToken jwtToken = token as JwtSecurityToken;

        return jwtToken;
    }   
    
    public static Dictionary<string, string> TokenToClaims(string token)
    {
        Dictionary<string, string> claims = [];
        
        JwtSecurityToken jwtToken = JwtToToken(token);
        if (jwtToken == null)
            return null;

        foreach (Claim claim in jwtToken.Claims)
        {
            claims.Add(claim.Type, claim.Value);
        }

        return claims;
    }

    public static bool ValidateJwtToken(string jwt)
    {
        JwtSecurityToken jwtToken = JwtToToken(jwt);
        Claim? expireClaim = jwtToken.Claims.SingleOrDefault((claim) => claim.Type.Equals("exp"));
        if (expireClaim == null)
            return false;

        bool timestampParsed = long.TryParse(expireClaim.Value, out long expireUnixTimestamp);
        if (!timestampParsed)
            return false;

        TimeSpan timeZoneOffset = TimeZoneInfo.Local.BaseUtcOffset;
        DateTime expTime = DateTimeOffset.FromUnixTimeSeconds(expireUnixTimestamp).DateTime + timeZoneOffset;
        if (expTime < DateTime.Now)
            return false;

        string issuer = jwtToken.Issuer;
        string? audience = jwtToken.Audiences.FirstOrDefault();

        if (string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(issuer))
            return false;

        if (!audience.Equals(issuer))
            return false;

        string issuerUrl = Defines.Constant.Url["Development"];
        if (!issuer.Equals(issuerUrl))
            return false;

        Claim? emailClaim = jwtToken.Claims.SingleOrDefault((claim) => claim.Type == ClaimTypes.Name);
        if (emailClaim == null)
            return false;

        if (string.IsNullOrEmpty(emailClaim.Value))
            return false;

        // TODO : Check equals to claim email value and input email for InputService
        return true;
    }
}
