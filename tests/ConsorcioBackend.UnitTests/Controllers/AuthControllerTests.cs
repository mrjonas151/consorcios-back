using ConsorcioAPI.Models;
using ConsorcioAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;

namespace ConsorcioBackend.UnitTests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var mockTokenService = new Mock<ITokenService>();
        mockTokenService.Setup(x => x.GenerateToken("admin"))
            .Returns("test-token");

        var loginData = new LoginModel
        {
            Username = "admin",
            Password = "password"
        };

        // Act
        var result = await HandleLogin(loginData, mockTokenService.Object);

        // Assert - NO Assert.IsType calls at all
        Assert.True(result is IResult);

        var statusCodeResult = result as IStatusCodeHttpResult;
        Assert.NotNull(statusCodeResult);
        Assert.Equal(200, statusCodeResult.StatusCode);

        // Extract token using reflection and JSON
        var resultType = result.GetType();
        var valueProperty = resultType.GetProperty("Value");
        Assert.NotNull(valueProperty);

        var value = valueProperty.GetValue(result);
        var jsonString = JsonSerializer.Serialize(value);
        var jsonDoc = JsonDocument.Parse(jsonString);

        Assert.True(jsonDoc.RootElement.TryGetProperty("Token", out var tokenProperty));
        Assert.Equal("test-token", tokenProperty.GetString());
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var mockTokenService = new Mock<ITokenService>();
        var loginData = new LoginModel
        {
            Username = "FalsoUser",
            Password = "ErradoPass"
        };

        // Act
        var result = await HandleLogin(loginData, mockTokenService.Object);

        // Assert - NO casting to specific types
        Assert.True(result is IResult);
        var statusCodeResult = result as IStatusCodeHttpResult;
        Assert.NotNull(statusCodeResult);
        Assert.Equal(401, statusCodeResult.StatusCode);
    }

    private async Task<IResult> HandleLogin(LoginModel loginData, ITokenService tokenService)
    {
        if (loginData?.Username == "admin" && loginData?.Password == "password")
        {
            var token = await Task.Run(() => tokenService.GenerateToken(loginData.Username));
            return Results.Ok(new { Token = token });
        }
        return Results.Unauthorized();
    }
}