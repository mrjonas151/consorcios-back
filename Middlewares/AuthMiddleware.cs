using Microsoft.AspNetCore.Http;
using ConsorcioAPI.Services;

namespace ConsorcioAPI.Middlewares;


public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;

    public AuthMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/auth"))
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var claimsPrincipal = _tokenService.ValidateToken(token);

            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
                await _next(context);
                return;
            }

            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Token inválido!");
            return;
        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Token ausente!");
        return;
    }
}