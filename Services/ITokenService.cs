using System.Security.Claims;

namespace ConsorcioAPI.Services;

public interface ITokenService
{
    string GenerateToken(string username);
    ClaimsPrincipal? ValidateToken(string token);
}