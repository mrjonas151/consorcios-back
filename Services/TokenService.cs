using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ConsorcioAPI.Services;

public class TokenService
{
    public string GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "user")
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidateAudience = true,
                ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}