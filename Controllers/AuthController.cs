using ConsorcioAPI.Models;
using ConsorcioAPI.Services;

namespace ConsorcioAPI.Controllers;

public static class AuthController
{
    public static RouteGroupBuilder MapAuth(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (LoginModel loginData, ITokenService tokenService) =>
        {
            if (loginData?.Username == "admin" && loginData?.Password == "password")
            {
                var token = await Task.Run(() => tokenService.GenerateToken(loginData.Username));
                return Results.Ok(new { Token = token });
            }
            return Results.Unauthorized();
        });

        return group;
    }
}