using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyWallet.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        // Por exemplo, você pode fazer log de informações sobre a solicitação
        Console.WriteLine("Middleware executado antes do controlador.");

        // Chama o próximo middleware na pipeline
        await _next(context);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder AuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}