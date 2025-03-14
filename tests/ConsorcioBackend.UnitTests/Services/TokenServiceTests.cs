using ConsorcioAPI.Services;
using System.Security.Claims;
using Xunit;

namespace ConsorcioBackend.UnitTests.Services;

public class TokenServiceTests
{
    private readonly ITokenService _tokenService;

    public TokenServiceTests()
    {
        Environment.SetEnvironmentVariable("JWT_KEY", "minha-chave-secreta-teste-teste-teste");
        Environment.SetEnvironmentVariable("JWT_ISSUER", "test-issuer");
        Environment.SetEnvironmentVariable("JWT_AUDIENCE", "test-audience");

        _tokenService = new TokenService();
    }

    [Fact]
    public void GenerateToken_ValidUsername_ReturnsToken()
    {
        var username = "admin";

        var token = _tokenService.GenerateToken(username);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void ValidateToken_ValidToken_ReturnsPrincipal()
    {
        var username = "admin";
        var token = _tokenService.GenerateToken(username);

        var principal = _tokenService.ValidateToken(token);

        Assert.NotNull(principal);
        var claim = principal.FindFirst(ClaimTypes.Name);
        Assert.NotNull(claim);
        Assert.Equal(username, claim.Value);
    }

    [Fact]
    public void ValidateToken_InvalidToken_ReturnsNull()
    {
        var invalidToken = "invalid-token";

        var result = _tokenService.ValidateToken(invalidToken);

        Assert.Null(result);
    }
}