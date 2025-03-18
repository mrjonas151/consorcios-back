using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace ConsorcioBackend.IntegrationTests;

public class TokenValidationTests
{
    [Fact]
    public void JWT_CreateAndValidate_WithValidKey_Succeeds()
    {
        string secretKey = "ThisIsASecretKeyWithAtLeast256BitsForTesting12345678901234";
        string issuer = "TestIssuer";
        string audience = "TestAudience";

        var token = CreateToken(secretKey, issuer, audience, 30);

        Assert.NotNull(token);
        Assert.NotEmpty(token);

        bool isValid = IsTokenValid(token, secretKey, issuer, audience);
        Assert.True(isValid);
    }

    private string CreateToken(string key, string issuer, string audience, int validForMinutes)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.UtcNow.AddMinutes(validForMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool IsTokenValid(string token, string key, string issuer, string audience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
