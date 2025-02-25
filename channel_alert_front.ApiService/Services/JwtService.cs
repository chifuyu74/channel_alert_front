using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using AutoMapper;

using channel_alert_front.ApiService.Configs;

namespace channel_alert_front.ApiService.Services;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly JwtConfig _jwtConfig;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public JwtService(IConfiguration configuration, IMapper mapper, ILoggerManager logger)
    {
        _configuration = configuration;
        _jwtConfig = new();
        _configuration.Bind(JwtConfig.Section, _jwtConfig);

        _mapper = mapper;
        _logger = logger;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        string key = _jwtConfig.Key;
        string issuer = _jwtConfig.Issuer!;
        string audience = _jwtConfig.Audience!;
        DateTime expires = DateTime.Now.AddSeconds(_jwtConfig.Expires);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        //new(JwtRegisteredClaimNames.Sub, userId),
        //new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //new(ClaimTypes.NameIdentifier, userId),

        JwtSecurityTokenHandler handler = new();
        string jwt = handler.WriteToken(token);
        return jwt;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = _jwtConfig.Issuer,
            ValidAudience = _jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key))
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
#pragma warning disable 8603
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            return null;
#pragma warning restore

        return principal;
    }

    public List<Claim> GenerateDefaultClaims(string email)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, "Role")
        ];

        return claims;
    }
}
