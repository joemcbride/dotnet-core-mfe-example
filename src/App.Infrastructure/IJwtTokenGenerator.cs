using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Infrastructure;

public class JwtOptions
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationInMinutes { get; set; } = 10;
}

public interface IJwtTokenGenerator
{
    string GenerateToken(IEnumerable<Claim> claims, JwtOptions options);
    ClaimsPrincipal ValidateToken(string token, JwtOptions options);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken(IEnumerable<Claim> claims, JwtOptions options)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature);

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(options.ExpirationInMinutes);

        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            IssuedAt = now,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = creds
        };

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.CreateToken(token);
        var stringToken = handler.WriteToken(jwtToken);
        return stringToken;
    }

    public ClaimsPrincipal ValidateToken(string token, JwtOptions options)
    {
        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(options.SecretKey)
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            },
            out var validatedToken
        );

        return principal;
    }
}
