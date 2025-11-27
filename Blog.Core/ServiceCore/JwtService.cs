using Blogs.Service.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blogs.Service.ServiceCore;
public interface IJwtService
{
    public List<Claim> GetClaims(User user);
    public string GetNewToken(User user);
}

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int LifetimeMinutes { get; set; }
    public SymmetricSecurityKey SecretKey { get; set; }
}

public class AuthOptions
{
    public const string JwtSectionAddress = "Authentication:Jwt";
    public const string JwtSecretKeySectionAddress = "Authentication:Jwt:SecretKey";
    public const string PasswordSectionAddress = "Authentication:Password";
}

public class JwtService(IOptions<JwtSettings> _jwtSettings) : IJwtService
{
    public List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim("UserId", user.Id.ToString()),
        };

        return claims;
    }

    public string GetNewToken(User user)
    {

        var claimsList = GetClaims(user);

        var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                notBefore: DateTime.UtcNow,
                claims: claimsList,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtSettings.Value.LifetimeMinutes)),
                signingCredentials: new SigningCredentials(_jwtSettings.Value.SecretKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
